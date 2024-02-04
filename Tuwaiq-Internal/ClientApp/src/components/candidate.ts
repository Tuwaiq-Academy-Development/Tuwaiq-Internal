import Swal from 'sweetalert2';
import { ILayout } from './layout';
import axios from 'axios';
import { Candidate } from '../types/global';
import * as noUiSlider from 'nouislider';
import 'nouislider/dist/nouislider.css';
import { PipsMode } from "nouislider/src/nouislider";
import Tagify from '@yaireo/tagify'

interface IComponent extends Partial<ILayout> {
    candidate: Candidate;
    questions: Question[];
    answers: { [key: string]: string | number | boolean };
    submitAnswers: (answers: []) => Promise<void>;
    showForm: boolean;
    showFormProfile: boolean;
    categoriesArray: string[];
}


type Question = {
    question: string;
    answers: string[] | number[];
    type?: string;
}


const component: IComponent = {
    answers: {},
    showForm: false,
    showFormProfile: false,
    questions: [
        { question: "ما تقييمك العام للمرشح", answers: [1, 2, 3, 4, 5] },
        { question: "ما تقييمك لمستوى اللغة الإنجليزية للمرشح", answers: [1, 2, 3, 4, 5] },
        { question: "ما تقييمك لمهارات التعلم الذاتي والمستمر للمرشح", answers: [1, 2, 3, 4, 5] },
        { question: "ما تقييمك لمهارات الاتصال والتواصل للمرشح", answers: [1, 2, 3, 4, 5] },
        { question: "ما تقييمك للمهارات الناعمة للمرشح", answers: [1, 2, 3, 4, 5] },
        { question: "ما تقييمك للمرشح بالجوانب التقنية ذات العلاقة بالوظيفة", answers: [1, 2, 3, 4, 5] },
        { question: "مهتم", answers: ["نعم", "لا"], type: "radio" },
    ],
    candidate: {
        id: "",
        name: null,
        nationalId: "",
        notes: "",
        tags: [],
        isSelected: false,
        companyId: "",
        company: null,
        createdAt: "",
        interviewedDate: "",
        questions: "",
        profileUrl: "",
    },
    categoriesArray: [],
    async init() {
        this.isLoading = true;
        this.questions.forEach(question => {
            this.answers[question.question] = question.answers[0];
        });
        try {
            const id = window.location.pathname.split("/")[2];
            const { data } = await axios.get(`/api/candidate/GetCandidate/`, {
                params: {
                    nationalId: id
                }
            });

            this.candidate = data;
            
            
            if(data?.name?.length > 0){
                this.showFormProfile = true;
            }
            
            if (data.questions && data.questions !== " ") {
                this.answers = JSON.parse(data?.questions);
            }

            const categories = await axios.get(`/api/category/Get/`);
            if (categories.data.length > 0) {
                this.questions.push({
                    question: "المجال المرشح له المتقدم",
                    answers: categories.data.map((category: any) => category.name)
                });
            }

            this.categoriesArray = categories.data.map((category: any) => category.name);
            this.isLoading = false;

        } catch (error) {
            console.log(error)
        }

        this.route = { url: "/candidate", name: "candidate" };


        document.querySelectorAll('[name=numberSlider]').forEach((el: any) => {
            noUiSlider.create(el, {
                start: Number(this.answers[el.attributes.question.value] || el.attributes.min.value),
                direction: 'rtl',
                connect: "lower",
                range: {
                    'min': Number(el.attributes.min.value),
                    'max': Number(el.attributes.max.value)
                },
                step: 1,
                pips: {
                    mode: PipsMode.Count,
                    values: Number(el.attributes.max.value),
                    stepped: true
                }
            }).on('update', (values: any) => {
                this.answers[el.attributes.question.value] = Number(values[0]);
            });

            var pips = el.querySelectorAll('.noUi-value');

            function clickOnPip(e: MouseEvent) {
                var value = Number((e.target as HTMLDivElement).getAttribute('data-value'));
                el.noUiSlider.set(value);
            }

            for (var i = 0; i < pips.length; i++) {

                // For this example. Do this in CSS!
                pips[i].style.cursor = 'pointer';
                pips[i].addEventListener('click', clickOnPip);
            }

        });

        this.$watch('showForm', (value: boolean) => {
            // if (value) {
                  new Tagify(this.$refs.tagifyElement, {
                        enforceWhitelist: false,
                        whitelist: this.categoriesArray,
                        dropdown: {
                            classname: "color-blue",
                            enabled: 0,              // show the dropdown immediately on focus
                            // maxItems: 5,
                            position: "text",         // place the dropdown near the typed text
                            closeOnSelect: false,          // keep the dropdown open after selecting a suggestion
                            highlightFirst: true
                        },
                        editTags: true
                    });

            this.$refs.tagifyElement?.addEventListener('change', (e: any) => {
                    this.answers["المجال المرشح له المتقدم"] = e.target.value;
                    console.log(e.target.value);
                    console.log(JSON.parse(e.target.value));
                    this.candidate.tags = JSON.parse(e.target.value).map((tag: any) => tag.value);
                }
            );  
            // }
        });



    },
    async submitAnswers(answers) {
        try {
            this.candidate.questions = JSON.stringify(answers);
            this.candidate.interviewedDate = new Date().toISOString();
            
            if (!this.candidate.tags) {
                this.candidate.tags = [];
            }
            
            //validate tags
            if(!this.answers["المجال المرشح له المتقدم"]) {
                Swal.fire({
                    title: 'يجب اختيار مجال واحد على الأقل',
                    icon: 'error',
                    confirmButtonText: 'موافق',
                    confirmButtonColor: 'red',
                });
                return;
            }
            
            if(!this.candidate.notes){
                Swal.fire({
                    title: 'يجب ادخال ملاحظات على المرشح',
                    icon: 'error',
                    confirmButtonText: 'موافق',
                    confirmButtonColor: 'red',
                });
                return;
            }
            
            
            await axios.put(`/api/candidate/Update`, this.candidate);

            Swal.fire({
                title: 'تم تقييم المرشح بنجاح',
                icon: 'success',
                confirmButtonText: 'موافق',
                confirmButtonColor: '#284387',
            }).then(() => {
                window.location.href = "/";
            });
        } catch (e) {
            Swal.fire({
                title: 'حدث خطأ أثناء تقييم المرشح',
                icon: 'error',
                confirmButtonText: 'موافق',
                confirmButtonColor: '#284387',
            });
        }
    }
};
export default () => component;
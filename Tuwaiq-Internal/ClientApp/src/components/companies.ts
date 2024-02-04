import { TabulatorFull as Tabulator } from 'tabulator-tables';
import { ILayout } from './layout';
import axios from 'axios';
import { Modal } from 'flowbite';
import type { ModalOptions, ModalInterface } from 'flowbite';
import type { InstanceOptions } from 'flowbite';
import Swal from 'sweetalert2';

interface IComponent extends Partial<ILayout> {
    createModal: ModalInterface | null;
    form: {
        name: string;
        logo: File | null;
        ids: string;
    };
    requestData: {
        name: string;
        logo: string;
        ids: string[];
    };
    createCompany(): void;
    table: Tabulator | null;
    deleteCompany(id: string): void;
}

const component: IComponent = {
    form: {
        name: '',
        logo: null,
        ids: ""
    },
    requestData: {
        name: '',
        logo: "",
        ids: []
    },
    table: null,
    createModal: null,
    async init() {
        this.isLoading = false;
        this.route = {
            name: 'الجهات',
            url: 'companies'
        }
        this.table = new Tabulator(this.$refs.companiesTable, {
            height: "100%",
            layout: "fitColumns",
            textDirection: "rtl",
            placeholder: "لا توجد بيانات",
            ajaxURL: `${document.location.origin}/api/company/Get`,
            ajaxLoader: true,
            headerSort: false,
            columns: [
                { title: '#', formatter: 'rownum', width: 40, hozAlign: 'center', headerSort: false, frozen: true },
                {
                    title: '', field: 'logo', widthGrow: 0.2,
                    headerSort: false,
                    formatter: (cell) => {
                        if (cell.getValue() === null || cell.getValue() === '') {
                            return `<img src="logo.jpeg" class="w-10 h-10 rounded-full">`;

                        }
                        return `<img src="/Storage/${cell.getValue()}" class="w-10 h-10 rounded-full">`;
                    }
                },
                { title: 'الجهة', field: 'name', headerSort: false },
                {
                    title: 'الإجراءات', field: 'id',
                    widthGrow: 0.3,
                    headerSort: false,
                    formatter: (cell) => {
                        return `<button class="bg-[#ffd0d0] hover:bg-[#ffd0d0]/70 px-5 py-1 rounded-lg text-[#D66A6A] font-normal duration-300 transition-colors ease-in-out" x-on:click="deleteCompany('${cell.getValue()}')">حذف الجهة</button>`;
                    }
                }
            ],
        });

        const modalOptions: ModalOptions = {
            placement: 'center',
            backdrop: 'dynamic',
            backdropClasses:
                'bg-gray-900/50 fixed inset-0 z-40',
            closable: true,
        };

        // instance options object
        const instanceOptions: InstanceOptions = {
            id: 'modal',
            override: true
        };

        this.createModal = new Modal(this.$refs.createCompanyModal, modalOptions, instanceOptions);

    },
    async createCompany() {
        const ids = this.form.ids.split(',');
        this.requestData.ids = ids;
        if (this.form.name === '' || this.form.name === null) {
            Swal.fire({
                icon: 'error',
                title: 'خطأ',
                text: 'الرجاء ادخال اسم الجهة',
            });
            return;
        }
        this.requestData.name = this.form.name;
        try {
            if (this.form.logo) {
                const formData = new FormData();
                formData.append('file', this.$refs.companyLogo.files[0]);
                const response = await axios.post(`${document.location.origin}/api/company/UploadLogo`, formData,
                    {
                        headers: {
                            "Content-Type": "multipart/form-data",
                        },
                    });
                this.requestData.logo = response.data;
            }
            const response = await axios.post(`${document.location.origin}/api/company/Create`, {
                Name: this.requestData.name,
                Logo: this.requestData.logo,
                Users: this.requestData.ids
            });

            if (response) {
                Swal.fire({
                    icon: 'success',
                    title: 'نجاح',
                    text: 'تم اضافة الجهة بنجاح',
                });
                this.createModal?.hide();
                this.table?.setData(null);
            }

        } catch (error) {
            Swal.fire({
                icon: 'error',
                title: 'خطأ',
                text: 'حدث خطأ اثناء اضافة الجهة',
            });
            console.log(error);
        }
    },
    async deleteCompany(id: string) {
        // confirm using sweet alert
        const { isConfirmed } = await Swal.fire({
            icon: 'warning',
            title: 'تأكيد',
            text: 'هل انت متأكد من حذف الجهة؟',
            showCancelButton: true,
            confirmButtonText: 'نعم',
            cancelButtonText: 'لا',
            confirmButtonColor: '#284387',
        });
        if (isConfirmed) {
            try {
                const response = await axios.delete(`${document.location.origin}/api/company/Delete/${id}`);
                if (response) {
                    Swal.fire({
                        icon: 'success',
                        title: 'نجاح',
                        text: 'تم حذف الجهة بنجاح',
                    });
                    this.table?.setData(null);
                }
            } catch (error) {
                Swal.fire({
                    icon: 'error',
                    title: 'خطأ',
                    text: 'حدث خطأ اثناء حذف الجهة',
                });
                console.log(error);
            }
        }
    }
};
export default () => component;
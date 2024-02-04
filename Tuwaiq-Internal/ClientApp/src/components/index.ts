import { TabulatorFull as Tabulator } from 'tabulator-tables';
import Swal from 'sweetalert2';
import { ILayout } from './layout';
import { Candidate } from '../types/global';
import axios from 'axios';

interface IComponent extends Partial<ILayout> {
    candidates: Candidate[];
    showSearchByIdModal: () => Promise<void>;
}

const component: IComponent = {
    candidates: [
        // {
        //     id: "123847191",
        //     name: 'احمد محمد',
        // },
        // {
        //     id: "3456789876",
        //     name: 'محمد احمد',
        // },
        // {
        //     id: "019239017",
        //     name: 'محمد علي',
        // }
    ],
    async init() {
        this.isLoading = false;
        this.route = {
            name: 'المرشحين',
            url: 'index'
        }
        const table = new Tabulator(this.$refs.candidatesTable, {
            height: "100%",
            layout: "fitColumns",
            textDirection: "rtl",
            placeholder: "لا توجد بيانات",
            pagination: true,
            paginationMode: "remote",
            paginationSize: 20,
            ajaxURL: `${document.location.origin}/api/candidate/Get`,
            ajaxLoader: true,
            dataSendParams: {
                "size": "pageSize", //change page request parameter to "pageNo"
            },
            dataReceiveParams: {
                'last_page': 'lastPage'
            },
            // paginationCounter: "rows",
            columns: [
                { title: '#', formatter: 'rownum', width: 40, hozAlign: 'center', headerSort: false, frozen: true },
                { title: 'الهوية', field: 'nationalId', headerSort: false },
                { title: 'تم التقييم', field: 'isReviewed', formatter: 'tickCross', hozAlign: 'center', headerSort: false, width: 100 },
                { title: 'مهتم', field: 'isSelected', formatter: 'tickCross', hozAlign: 'center', headerSort: false, width: 80 },
                // { title: 'الأسم', field: 'name' },
            ],
        });

        table.on('rowClick', (e, row) => {
            const data = row.getData();
            window.location.href = `/candidate/${data.nationalId}`;
        });

    },
    async showSearchByIdModal() {
        await Swal.fire({
            title: 'أدخل رقم الهوية',
            input: 'text',
            inputLabel: 'رقم الهوية',
            inputPlaceholder: 'أدخل رقم الهوية',
            inputValue: '',
            cancelButtonText: 'إلغاء',
            confirmButtonText: 'بحث',
            confirmButtonColor: '#6A5C9F',
            showLoaderOnConfirm: true,
            showCancelButton: true,
            inputValidator: (value) => {
                if (!value) {
                    return 'يجب أدخال رقم الهوية'
                }
            },
            preConfirm: async (id) => {
                try {
                    const response = await axios.get(`${document.location.origin}/api/candidate/GetCandidate`, {
                        params: {
                            nationalId: id,
                        }
                    });

                    if (!response || response.status !== 200 || !response.data) {
                        Swal.fire({
                            icon: 'error',
                            title: 'خطأ',
                            text: 'لم يتم العثور على مرشح بهذا الرقم',
                        })
                        return;
                    }

                    window.location.href = `/candidate/${response.data.nationalId}`;
                } catch (error) {
                    Swal.showValidationMessage(`لم يتم العثور على مرشح بهذا الرقم`);
                }
            },
        })

        // if (id) {
        //     try {
        //
        //         const response = await axios.get(`${document.location.origin}/api/candidate/GetCandidate`, {
        //             params: {
        //                 nationalId: id,
        //             }
        //         });
        //
        //         if (!response || response.status !== 200 || !response.data) {
        //             Swal.fire({
        //                 icon: 'error',
        //                 title: 'خطأ',
        //                 text: 'لم يتم العثور على مرشح بهذا الرقم',
        //             })
        //             return;
        //         }
        //
        //         window.location.href = `/candidate/${response.data.nationalId}`;
        //     }
        //     catch (e) {
        //         Swal.fire({
        //             icon: 'error',
        //             title: 'خطأ',
        //             text: 'لم يتم العثور على مرشح بهذا الرقم',
        //         })
        //     }
        // }
    }
};
export default () => component;
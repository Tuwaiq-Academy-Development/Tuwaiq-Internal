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
    };
    createCategory(): void;
    table: Tabulator | null;
    deleteCategory(id: string): void;
}

const component: IComponent = {
    form: {
        name: '',
    },
    table: null,
    createModal: null,
    async init() {
        this.isLoading = false;
        this.route = {
            name: 'التخصصات',
            url: 'categories'
        }
        this.table = new Tabulator(this.$refs.categoriesTable, {
            height: "100%",
            layout: "fitColumns",
            textDirection: "rtl",
            placeholder: "لا توجد بيانات",
            ajaxURL: `${document.location.origin}/api/category/Get`,
            ajaxLoader: true,
            headerSort: false,
            columns: [
                { title: 'التخصص', field: 'name', headerSort: false },
                {
                    title: 'الإجراءات', field: 'id',
                    widthGrow: 0.3,
                    headerSort: false,
                    formatter: (cell) => {
                        return `<button class="bg-[#ffd0d0] hover:bg-[#ffd0d0]/70 px-5 py-1 rounded-lg text-[#D66A6A] font-normal duration-300 transition-colors ease-in-out" x-on:click="deleteCategory('${cell.getValue()}')">حذف التخصص</button>`;
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

        this.createModal = new Modal(this.$refs.createCategoryModal, modalOptions, instanceOptions);

    },
    async createCategory() {
        if (this.form.name === '' || this.form.name === null) {
            Swal.fire({
                icon: 'error',
                title: 'خطأ',
                text: 'الرجاء ادخال التخصص',
            });
            return;
        }
        try {
            const response = await axios.post(`${document.location.origin}/api/category/Create`, {
                name: this.form.name,
            });

            if (response) {
                Swal.fire({
                    icon: 'success',
                    title: 'نجاح',
                    text: 'تم اضافة التخصص بنجاح',
                });
                this.createModal?.hide();
                this.table?.setData(null);
            }

        } catch (error) {
            Swal.fire({
                icon: 'error',
                title: 'خطأ',
                text: 'حدث خطأ اثناء اضافة التخصص',
            });
            console.log(error);
        }
    },
    async deleteCategory(id: string) {
        // confirm using sweet alert
        const { isConfirmed } = await Swal.fire({
            icon: 'warning',
            title: 'تأكيد',
            text: 'هل انت متأكد من حذف التخصص',
            showCancelButton: true,
            confirmButtonText: 'نعم',
            cancelButtonText: 'لا',
            confirmButtonColor: '#284387',
        });
        if (isConfirmed) {
            try {
                const response = await axios.delete(`${document.location.origin}/api/category/Delete/${id}`);
                if (response) {
                    Swal.fire({
                        icon: 'success',
                        title: 'نجاح',
                        text: 'تم حذف التخصص بنجاح',
                    });
                    this.table?.setData(null);
                }
            } catch (error) {
                Swal.fire({
                    icon: 'error',
                    title: 'خطأ',
                    text: 'حدث خطأ اثناء حذف التخصص',
                });
                console.log(error);
            }
        }
    }
};
export default () => component;
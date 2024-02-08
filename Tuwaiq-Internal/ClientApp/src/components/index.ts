import {ILayout} from './layout';
import {any} from "zod";
import * as ExcelJS from "exceljs";
import {TabulatorFull as Tabulator} from "tabulator-tables";
import {convertToDateStr} from "../helpers/convertToDate";
import {myAxios} from "../config/axiosConfig";
import dayjs from "dayjs";
import {api} from '../utils/endpoints';
import {BASE_URL} from "../config/envConfig";

interface IComponent extends Partial<ILayout> {
    initTabulator(): void;
    candidates: string;
    file: any;
    table: Tabulator | null;
    checkStatus(id): Promise<void>;
    exportExcel(): any;

}

const component: IComponent = {
    file: any,
    table: null,
    candidates: "",
    async init() {
        this.setCurrentRoute!('index', 'index');
        this.initTabulator();
    },
    initTabulator() {
        const template = document.createElement('template');
        template.innerHTML = '<div style="display:inline-block;" class="d-flex flex-row">' +
            '<div>جاري التحميل... </div>' +
            '<div class="ml-2 activity-sm" data-role="activity" data-type="atom" data-style="dark"></div>' +
            '</div>';
        const dataLoaderLoading = template.content.firstChild as HTMLElement;

        this.table = new Tabulator(this.$refs.table, {
            height: "100%",
            layout: 'fitColumns',
            textDirection: "rtl",
            ajaxURL: `${window.base_url}api/Checks/GetHistory`,
            pagination: true,
            paginationMode: "remote",
            paginationSize: 10,
            ajaxConfig: {
                method: "GET",
                headers: {
                    "my-x-12s4": `${window.token}`,
                },
            },
            ajaxFiltering: false,
            ajaxSorting: false,
            ajaxParams: () => {
                return {
                    // query: this.model_search
                };
            },
            ajaxLoader: true,
            placeholder: "لا توجد بيانات",
            dataLoaderLoading: dataLoaderLoading,
            columns: [
                // {
                //     title: 'رقم المستخدم', field: 'userId', headerSort: false,
                //     formatter: function (cell) {
                //         return `
				// 		<div class="flex justify-center items-center">
				// 			<div class="text-black leading-5">
				// 				${cell.getValue()}
				// 			</div>
				// 		</div>`
                //     }
                // },
                {
                    title: 'اسم المستخدم', field: 'firstName', headerSort: false,
                    formatter: function (cell) {
                        if (cell.getValue() == null) return '';
                        return `
						<div class="flex justify-center items-center">
							<div class="text-black leading-5">
								${cell.getValue()}
							</div>
						</div>`
                    }
                },
                // {
                //     title: 'الرابط', field: 'fileUrl', headerSort: false,
                //     formatter: function (cell) {
                //         if (cell.getValue() == null) return '';
                //         return `
                // 		<div class="flex justify-center items-center">
                // 			<div class="text-black leading-5">
                // 				${cell.getValue()}
                // 			</div>
                // 		</div>`
                //     }
                // },
                {
                    title: 'الحاله', field: 'status', headerSort: false,
                    width: 200,
                    formatter: function (cell) {
                        if (cell.getValue() == null) return '';
                        return `
						<div class="flex justify-center items-center">
							<div class="text-black leading-5">
								${cell.getValue()}
							</div>
						</div>`
                    }
                },

                {
                    title: 'تاريخ الانشاء', field: 'createdOn', headerSort: false,
                    width: 300,
                    formatter: function (cell) {
                        if (cell.getValue() == null) return '';
                        return `
						<div class="flex justify-center items-center">
							<div class="text-black leading-5" dir="ltr">
								${dayjs(cell.getValue()).format('DD-MM-YYYY hh:mm a')}
							</div>
						</div>`
                    }
                },

                {
                    title: 'تاريخ اخر تحديث', field: 'lastUpdate', headerSort: false,
                    width: 300,
                    formatter: function (cell) {
                        if (cell.getValue() == null) return '';
                        return `
						<div class="flex justify-center items-center">
							<div class="text-black leading-5" dir="ltr">
								${dayjs(cell.getValue()).format('DD-MM-YYYY hh:mm a')}
							</div>
						</div>`
                    }
                },
                {
                    title: 'نوع الاستعلام', field: 'type', headerSort: false,
                    width: 200,
                    formatter: function (cell) {
                        if (cell.getValue() == null) return '';
                        return `
						<div class="flex justify-center items-center">
							<div class="text-black leading-5">
							<template x-if="'${cell.getValue()}'==1">
							<div class="text-black leading-5">
								التأمينات الاجتماعية
							</div>
					         </template>
							</div>
						</div>`
                    }
                },
                //action
                {
                    title: 'الإجراءات', headerSort: false, width: 150,
                    cellClick: (e, cell) => {
                        const data = cell.getRow().getData();
                        this.checkStatus(cell.getRow().getData().id);
                    },
                    formatter: function (cell) {
                       const data = cell.getRow().getData().status.split("/");
                       if(data[0] == data[1]) {
                           return 'تم الانتهاء'
                       }
                        return `
                        <div class="flex justify-center items-center"> 
                                <button  class="w-full h-7 rounded-full flex justify-center items-center bg-gray-200 hover:bg-gray-400"  >تحديث </button> 
 <a target="_blank" :href="exportExcel()"  class="w-full h-7 rounded-full flex justify-center items-center bg-gray-200 hover:bg-gray-400">
                    تصدير
                </a>                            </div>   `
                    }
                },


            ],
        });

        // this.table.on("rowClick", (e, row) => {
        //     if (e.target.classList.contains("w-7")) return;
        //     location.href = document.location.origin + "/" + "Initiatives/InitiativePublishes/" + row.getData()["id"];
        // })


    },
    async checkStatus(id) {
        await myAxios.post(`api/Checks/UpdateStatus?id=` + id);
        this.table?.replaceData();
    } ,
    async exportExcel() {
        return   BASE_URL  +api.ExportExcelUrl ;
     }
};
export default () => component;
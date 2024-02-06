import {ILayout} from './layout';
import axios from 'axios';
import {any} from "zod";
import {toastError, toastSuccess} from "../../config/toastifyConfig";
import * as ExcelJS from "exceljs";
import {TabulatorFull as Tabulator} from "tabulator-tables";
import {convertToDateStr} from "../helpers/convertToDate";

interface IComponent extends Partial<ILayout> {
    initTabulator(): void;

    candidates: string;
    file: any;
    table: Tabulator | null;

    checkStatus( id): void;
}

const component: IComponent = {
    file: any,
    table: null,
    candidates: "",
    async init() {
        this.setCurrentRoute!('index', 'index');

        this.initTabulator();
    },

    initTabulator: function () {
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
            ajaxURL: `${document.location.origin}/api/Candidate/GetHistory`,
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
                {
                    title: 'رقم المستخدم', field: 'userId', headerSort: false,
                    formatter: function (cell) {
                        return `
						<div class="flex justify-center items-center">
							<div class="text-black leading-5">
								${cell.getValue()}
							</div>
						</div>`
                    }
                },
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
                {
                    title: 'الرابط', field: 'fileUrl', headerSort: false,
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
                    title: 'الحاله', field: 'status', headerSort: false,
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
                    formatter: function (cell) {
                        if (cell.getValue() == null) return '';
                        return `
						<div class="flex justify-center items-center">
							<div class="text-black leading-5">
								${convertToDateStr(cell.getValue())}
							</div>
						</div>`
                    }
                },

                {
                    title: 'تاريخ اخر تحديث', field: 'lastUpdate', headerSort: false,
                    formatter: function (cell) {
                        if (cell.getValue() == null) return '';
                        return `
						<div class="flex justify-center items-center">
							<div class="text-black leading-5">
								${convertToDateStr(cell.getValue())}
							</div>
						</div>`
                    }
                },
                {
                    title: 'نوع المستعلم', field: 'type', headerSort: false,
                    formatter: function (cell) {
                        if (cell.getValue() == null) return '';
                        return `
						<div class="flex justify-center items-center">
							<div class="text-black leading-5">
							<template x-if="'${cell.getValue()}'==1">
							<div class="text-black leading-5">
								Hadaf
							</div>
					         </template>
							</div>
						</div>`
                    }
                },
                //action
                {
                    title: 'الإجراءات', headerSort: false,
                    cellClick: (e, cell) => {
                        const data = cell.getRow().getData();
                        this.checkStatus(cell.getRow().getData().id);

                    },
                    formatter: function (cell) {
                        return `
                        <div class="flex justify-center items-center"> 
                                <button  class="btn-primary"  >check </button> 
                            </div>`
                    }
                },


            ],
        });

        // this.table.on("rowClick", (e, row) => {
        //     if (e.target.classList.contains("w-7")) return;
        //     location.href = document.location.origin + "/" + "Initiatives/InitiativePublishes/" + row.getData()["id"];
        // })


    },
    checkStatus: function (id) {
       console.log(id);
        axios.post(`/api/Candidate/UpdateStatus?id=`+id);
        this.initTabulator();
        this.table.setData();
    }
};
export default () => component;
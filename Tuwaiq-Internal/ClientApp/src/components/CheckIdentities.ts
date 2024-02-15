import {ILayout} from './layout';
import axios from 'axios';
import {any} from "zod";
import * as ExcelJS from "exceljs";
import {toastError, toastSuccess} from "../config/toastifyConfig";

interface IComponent extends Partial<ILayout> {

    checkIdentities: () => Promise<void>;
    handleImportChange: (e: any) => void;
    candidates: string;
    file: any;
    files: [];
}

const CheckIdentities: IComponent = {
    file: any,
    files:  [],
    candidates: "",
    async init() {
        this.setCurrentRoute!('CheckIdentities', 'CheckIdentities');

    },
    async checkIdentities() {
        this.isLoading = true;
        var model = {
            nationalIds: this.candidates.split('\n'),
            FilePath: ""
        }
        if (this.files.length > 0) {
            const response = await this.uploadFile!(this.files);  
            model.FilePath= response.data.fileNames[this.file.name];
         }
        try {
            var check = await axios.post(`/api/Checks/CheckIdentities`, model);
            if(check.status === 200){
            toastSuccess('تم الاستعلام علي الهويات بنجاح');
            window.location.href = "/";
            }else{
                toastError(check.data ?? 'خطأ في الاستعلام علي الهويات');
            }
        }catch (e :any) {
            toastError(e?.response?.data ?? 'خطأ في الاستعلام علي الهويات');
            console.log(e);
        }
        this.isLoading = false;
    },
    async handleImportChange(e: any) {
        this.file = e.target.files[0];
        this.files = e.target.files;
        e.preventDefault();
        const wb = new ExcelJS.Workbook();
        const reader = new FileReader();
        reader.readAsArrayBuffer(this.file);
        reader.onload = () => {
            const buffer = <ArrayBuffer>reader.result;
            let localcandidates = "";
            wb.xlsx.load(buffer).then(workbook => {
                workbook.worksheets[0].eachRow(function (row, rowNumber) {
                    if (rowNumber !== 1) {
                        localcandidates += row.values[1] + '\n';
                    }
                })
            }).then(() => {
                toastSuccess('تم إستيراد الهويات بنجاح');
                this.candidates = localcandidates;
            })
        }
    },

};
export default () => CheckIdentities;
import {ILayout} from './layout';
import axios from 'axios';
import {any} from "zod";
import * as ExcelJS from "exceljs";
import {toastSucess} from "../config/toastifyConfig";

interface IComponent extends Partial<ILayout> {

    checkIdentities: () => Promise<void>;
      handleImportChange: (e: any) => void;
    candidates: string;
    file: any;
    files:any;
}

const CheckIdentities: IComponent = {
    file: any,
    files:any,
    candidates: "",
    async init() {
        this.setCurrentRoute!('CheckIdentities', 'CheckIdentities');

    },
    async checkIdentities() {
        this.isLoading = true;
        
        const response = await this.uploadFile!(this.files);
    var model={
        nationalIds: this.candidates.split('\n'),

        FilePath: response.data.fileNames[this.file.name],
        
    }
        await axios.post(`/api/Checks/CheckIdentities`,model);
        toastSucess('تم الاستعلام علي الهويات بنجاح');
        window.location.href = "/";
        this.isLoading = false;
    },
  async  handleImportChange(e: any) {
        this.file = e.target.files[0];
        this.files=e.target.files;
        e.preventDefault();
        const wb = new ExcelJS.Workbook();
        const reader = new FileReader();
        reader.readAsArrayBuffer(this.file);
        reader.onload = () => {
            const buffer = <ArrayBuffer>reader.result;
            let localcandidates = "";
            wb.xlsx.load(buffer).then(workbook => {
                workbook.eachSheet((sheet, id) => {
                    sheet.eachRow(function (row, rowNumber) {
                        if (rowNumber !== 1) {
                            row.eachCell((cell) => {
                                localcandidates += cell.text + '\n';
                            })

                        }
                    })
                })
            }).then(() => {
                toastSucess('تم إستيراد الهويات بنجاح');
                this.candidates = localcandidates;
            })
        }
    },

};
export default () => CheckIdentities;
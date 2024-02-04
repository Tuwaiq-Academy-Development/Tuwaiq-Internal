import { Alpine as AlpineType } from 'alpinejs'

declare global {
    var Alpine: AlpineType
}

interface Candidate {
    id: string;
    name: string | null | undefined;
    nationalId: string | null | undefined;
    notes: string | null | undefined;
    tags: string[] | null | undefined;
    isSelected: boolean | null | undefined;
    companyId: string | null | undefined;
    company: Company | null | undefined;
    createdAt: string | null | undefined; // Use string for ISO 8601 date-time format
    interviewedDate: string | null | undefined; // Use string for ISO 8601 date-time format
    questions: string | null | undefined;
    profileUrl: string | null | undefined;
}

interface Company {
    id: string;
    name: string;
    candidates: Candidate[];
    createdAt: string; // Use string for ISO 8601 date-time format
}

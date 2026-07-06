export interface Book
{
    id: number;
    title: string;
    author: string;
    category: string;
    noOfAvailableCopies: number;
    imageUrl:string | null;
}
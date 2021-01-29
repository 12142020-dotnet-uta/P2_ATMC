import { Picture } from "./picture";

export interface PagedResponse {
  pageNumber:number
  pageSize:number
  //uri
  firstPage:string
  //uri
  lastPage:string
  totalPages:number
  totalRecords:number
  //uri
  next:string
  //uri
  previous:string

  data:Picture[]
  succeeded:boolean

}

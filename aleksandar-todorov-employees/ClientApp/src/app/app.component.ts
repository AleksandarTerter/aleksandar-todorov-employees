import { HttpClient, HttpParams } from '@angular/common/http';
import { Component, Inject } from '@angular/core';
import { FormControl } from '@angular/forms';
import { Observable, map, of, startWith } from 'rxjs';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html'
})
export class AppComponent {

  public title = 'app';
  public fileName = '';
  public result = '';
  private baseUrl: string;
  myControl = new FormControl('d-M-yyyy');
  options: string[] = ['yyyy-MM-dd', 'yyyy-dd-MM', 'dd-MM-yyyy', 'dd/MM/yyyy', 'MM/dd/yyyy', 'dd/MM/yy'];
  filteredOptions: Observable<string[]> = of([]);
  displayedColumns: string[] = [
    'empAID',
    'empBID',
    'projectID',
    'daysWorked',
  ];
  dataSource: DuoWorkPerProject[] = [];

  constructor(private http: HttpClient, @Inject('BASE_URL') baseUrl: string) {
    this.baseUrl = baseUrl;
  }


  ngOnInit() {
    this.filteredOptions = this.myControl.valueChanges.pipe(
      startWith(''),
      map(value => this._filter(value || '')),
    );
  }

  private _filter(value: string): string[] {
    const filterValue = value.toLowerCase();

    return this.options.filter(option => option.toLowerCase().includes(filterValue));
  }

  onFileSelected(event: any) {

    const file: File = event.target.files[0];

    if (file) {
      this.fileName = file.name;
      const formData = new FormData();
      formData.append("file", file);
      const params = new HttpParams().append("datePattern", this.myControl.value ?? '');

      this.dataSource = [];
      this.http.post<DuoWorkPerProjectDto[]>(this.baseUrl + 'employees/read-employees-csv', formData, { params })
        .pipe(
          map(r => r.map<DuoWorkPerProject>(i => ({ empAID: i.duo.empAID, empBID: i.duo.empBID, daysWorked: i.daysWorked, projectID: i.projectID })))
        )
        .subscribe(rows => this.dataSource = rows);
    }
  }
}

export interface DuoWorkPerProject {
  empAID: number
  empBID: number
  projectID: number
  daysWorked: number
}

export interface DuoWorkPerProjectDto {
  duo: DuoDto
  projectID: number
  daysWorked: number
}

export interface DuoDto {
  empAID: number
  empBID: number
}

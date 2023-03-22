import { Component, Inject } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { Client } from './api.service';
import { fileTypeValidator } from './file-type-validator.directive';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent {
  constructor( @Inject(Client) private readonly apiClient: Client) {
  }

  fileUploadForm = new FormGroup({
    Email: new FormControl('', [
      Validators.required, 
      Validators.pattern(/^([a-zA-Z0-9_\-\.]+)@([a-zA-Z0-9_\-\.]+)\.([a-zA-Z]{2,5})$/)]),
    Document: new FormControl('', [Validators.required]),
    DocumentSource: new FormControl(null, [
      fileTypeValidator("application/vnd.openxmlformats-officedocument.wordprocessingml.document")])
  })

  get email() {
    return this.fileUploadForm.get('Email')
  }

  get document() {
    return this.fileUploadForm.get('Document')
  }

  get documentSource() {
    return this.fileUploadForm.get('DocumentSource')
  }

  // u@i.ua
  onFilePicked(event: any){
    if (event.target.files && event.target.files.length) {
      const [file] = event.target.files;
      this.fileUploadForm.patchValue({
        DocumentSource: file
      });
      console.log(`My file : ${file}`);
    }
  }

  isSucceed?: boolean 
  isLoading: boolean = false

  upload(){
    const email: string = this.email!.value!
    const file: File = this.fileUploadForm.get('DocumentSource')!.value!
    
    this.isLoading = true

    this.apiClient
    .docs(email, {fileName: `${email}__${file.name}`, data: file})
    .subscribe(()=>{
      console.log('docs executed');
      this.isSucceed = true
      this.isLoading = false
    },
    (error)=> {
      this.isSucceed = false
      this.isLoading = false
    })
  }

  onOk(){
    this.fileUploadForm.reset()
    this.isSucceed = undefined
  }
}
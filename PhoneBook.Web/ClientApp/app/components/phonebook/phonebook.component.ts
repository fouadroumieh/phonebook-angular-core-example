import { Component, Inject, ViewChild } from '@angular/core';
import { Http } from '@angular/http';
import { PhoneBook } from './phonebook';
import { CreateContact } from './createcontact';
import { NgForm } from '@angular/forms';
import { NotificationsService } from 'angular4-notify';

@Component({
    selector: 'phonebook',
    templateUrl: './phonebook.component.html'
})

export class PhoneBookComponent {
    phonebook: PhoneBook[];
    newContact: any = {};
    entryTypes: string[] = new Array("Mobile", "Work", "Fax");
    @ViewChild('phoneBookfrm') phoneBookfrm: NgForm;

    constructor(private http: Http, @Inject('BASE_URL') private baseUrl: string, protected notificationsService: NotificationsService) {
        this.getPhonebook();
    }

    getPhonebook() {
        this.http.get(this.baseUrl + 'api/PhoneBook/Get').subscribe(result => {
            this.phonebook = result.json() as PhoneBook[];
        }, error => this.notificationsService.addError('Error happened, please contact admin.'));
    }

    saveContact(contact: CreateContact) {
        if (!this.phoneBookfrm.valid) {
            Object.keys(this.phoneBookfrm.controls).forEach(field => {
                const control = this.phoneBookfrm.controls[field];
                control.markAsTouched({ onlySelf: true });
            });
            return;
        }
        this.http.post(this.baseUrl + 'api/PhoneBook/Post', contact).subscribe(result => {
            this.getPhonebook();
            this.cancelContact();
            this.notificationsService.addInfo('Contact created successfully.');
        }, error => {
            if (error.status === 409)
                this.notificationsService.addError("Entry already exists, for this contact.");
            else if (error.status === 400) {
                var res = error.json();
                for (var key in res) {
                    if (res[key].hasOwnProperty("errorMessage"))
                        this.notificationsService.addError(res[key].errorMessage);
                }
            }
            else
                this.notificationsService.addError("Error happened, please contact admin.");
        });
    }

    cancelContact() {
        this.phoneBookfrm.reset();
        this.newContact = {};
    }
}
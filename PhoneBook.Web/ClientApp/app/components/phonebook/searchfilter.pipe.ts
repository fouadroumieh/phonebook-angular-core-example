import { Injectable, Pipe, PipeTransform } from '@angular/core';

@Pipe({
    name: 'searchfilter'
})

@Injectable()
export class SearchFilterPipe implements PipeTransform {
    transform(items: any[], field: string, value: string): any[] {
        if (!items) return [];
        if (!value) return items;
        value = value.toLocaleLowerCase();
        return items.filter(it => {
            return it.name.toLocaleLowerCase().includes(value);
        });
    }
}
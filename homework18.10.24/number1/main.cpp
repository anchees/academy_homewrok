#include <iostream>
#include <iomanip>
#include <conio.h>
#include "drob.h"
using namespace std;

void operation(drob drob1, drob drob2, string op, drob res){
    if (op=="+"){
        res.plus(drob1, drob2, res);
    }else if (op=="-"){
        res.minus(drob1, drob2, res);
    }else if (op=="*"){
        res.umnoz(drob1, drob2, res);
    }else if (op=="/"){
        res.del(drob1, drob2, res);
    }else
        cout<<"incorrect operation";
}

int main(){
    drob drob1, drob2, res;
    string op;
    cout <<"Drob 1:"<<endl;
    drob1.vvod();
    drob1.vivod();
    cout <<"Drob 2:"<<endl;
    drob2.vvod();
    drob2.vivod();
    cout <<"Enter operation (+,-,*,/) -> ";
    cin >> op;
    operation(drob1,drob2,op,res);
    _getch();
    return 0;
}
#include <iostream>
#include "drob.h"
using namespace std;

void drob::vvod(){
    cout<<"Enter ch -> ";
    cin>>ch;
    cout<<"Enter zn ->";
    cin>>zn;
}

void drob::vivod(){
    cout<<ch<<"/"<<zn<<endl;
}

void drob::plus(drob drob1, drob drob2, drob res){
    res.ch = drob1.ch * drob2.zn + drob1.zn * drob2.ch;
    res.zn = drob1.zn * drob2.zn;
    res.vivod();
}

void drob::minus(drob drob1, drob drob2, drob res){
    res.ch = drob1.ch * drob2.zn - drob1.zn * drob2.ch;
    res.zn = drob1.zn * drob2.zn;
    res.vivod();
}

void drob::umnoz(drob drob1, drob drob2, drob res){
    res.ch = drob1.ch * drob2.ch;
    res.zn = drob1.zn * drob2.zn;
    res.vivod();
}

void drob::del(drob drob1, drob drob2, drob res){
    res.ch = drob1.ch * drob2.zn;
    res.zn = drob1.zn * drob2.ch;
    res.vivod();
}


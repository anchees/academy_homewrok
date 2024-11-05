#include <iostream>
#include <iomanip>
#include <conio.h>
#include <fstream>
#include "phonebook.h"
using namespace std;

void phoneb::delet(){
    name=(nullptr);
    homep=("");
    workp=("");
    mobilep=(""); 
    moreinfo=("");
}

void phoneb::vivod(){
    if (name!=nullptr){
        cout<<"-------------------------------"<<endl;
        cout<<"Full name: "<< getname() << endl;
        cout<<"Home phone: "<< gethomep() << endl;
        cout<<"Work phone: "<< getworkp() << endl;
        cout<<"Mobile phone: "<< getmobilep() << endl;
        cout<<"More info: "<< getmoreinfo() << endl; 
    } else {
        cout<<"ghost"<<endl;
    }
}

void phoneb::filevvod(ifstream &book, int proverka){   
    string musor="";
    getline(book, musor);
    if (musor!="" && proverka==0){
        getline(book, nname); 
        getline(book, homep);
        getline(book, workp);
        getline(book, mobilep);
        getline(book, moreinfo);
        name = new char[nname.length() + 1];
        strcpy(name, nname.c_str());
    }else if (getname()==nullptr && proverka>0) {
        cout<<"Enter full name: ";
        getline(cin, nname);
        cout<<"Enter home phone: ";
        getline(cin, homep);
        cout<<"Enter work phone: ";
        getline(cin, workp);
        cout<<"Enter mobile phone: ";
        getline(cin, mobilep);
        cout<<"Enter more info: ";
        getline(cin, moreinfo);
        name = new char[nname.length() + 1];
        strcpy(name, nname.c_str()); 
        proverka=0;
    }
}

void phoneb::filevivod(ofstream &bookin){
    if (name!=nullptr){
        bookin<<"-------------------------------" << endl;
        bookin<< name << endl;
        bookin<< homep << endl;
        bookin<< workp << endl;
        bookin<< mobilep << endl;
        bookin<< moreinfo << endl;
    }
}


// void phoneb::vvod(){
//     cout<<"Enter full name: ";
//     getline(cin, nname);
//     cout<<"Enter home phone: ";
//     getline(cin, homep);
//     cout<<"Enter work phone: ";
//     getline(cin, workp);
//     cout<<"Enter mobile phone: ";
//     getline(cin, mobilep);
//     cout<<"Enter more info: ";
//     getline(cin, moreinfo);
//     name = new char[nname.length() + 1];
//     strcpy(name, nname.c_str()); 
// }

// out<<"Full name: "<< name << endl;
    // out<<"Home phone: "<< homep << endl;
    // out<<"Work phone: "<< workp << endl;
    // out<<"Mobile phone: "<< mobilep << endl;
    // out<<"More info: "<< moreinfo << endl;
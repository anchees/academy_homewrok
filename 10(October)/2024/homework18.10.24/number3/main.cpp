#include <iostream>
#include "class.h"
using namespace std;

int String::scet=0;

int main(){
    String mem1;
    mem1.random();
    mem1.print();
    cout<<"\nCreated str: "<<String::getscet()<<endl;

    cout<<endl;

    int ku;
    cout<<"Enter len -> ";
    cin>>ku;
    String mem2{ku};
    mem2.random();
    mem2.print();
    cout<<"\nCreated str: "<<String::getscet()<<endl;

    cout<<endl;

    string strokaP;
    cout<<"Enter stroka: ";
    cin>>strokaP;
    String mem3{1, strokaP};
    mem3.print();
    cout<<"\nCreated str: "<<String::getscet()<<endl;

    cout<<endl;

    cout<<"Udalenie strok..."<<endl;
    cout<<"Created str: "<<String::getscet()<<endl;
    mem1.~String();
    cout<<"Created str: "<<String::getscet()<<endl;
    mem2.~String();
    cout<<"Created str: "<<String::getscet()<<endl;
    mem3.~String();
    cout<<"Created str: "<<String::getscet()<<endl;
    return 0;
}
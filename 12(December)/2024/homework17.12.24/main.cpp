#include <iostream>
#include "point.h"
using namespace std;

int main() {
    point A(5, 5), B(3, 3);
    cout<<"A:\t";
    A.print();
    cout<<"B:\t";
    B.print();

    B = A;
    cout<<"\nB=A:\n";
    cout<<"A:\t";
    A.print();
    cout<<"B:\t";
    B.print();

    --A;
    cout<<"\n--A\n";
    cout<<"A:\t";
    A.print();
    cout<<"B:\t";
    B.print();

    A--;
    cout<<"\nA--\n";
    cout<<"A:\t";
    A.print();
    cout<<"B:\t";
    B.print();

    return 0;
}
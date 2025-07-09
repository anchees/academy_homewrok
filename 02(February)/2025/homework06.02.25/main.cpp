#include <iostream>
#include "Point.h"
using namespace std;

int main() {
    Point<int> p1(1, 2);
    Point<int> p2(3, 4);

    Point<int> sum1 = p1 + p2;    
    Point<int> sum2 = p1 + 5;     
    Point<int> sum3 = 10 + p1;    

    cout << "p1 + p2 = "; sum1.print();
    cout << "p1 + 5  = "; sum2.print();
    cout << "10 + p1 = "; sum3.print();

    return 0;
}

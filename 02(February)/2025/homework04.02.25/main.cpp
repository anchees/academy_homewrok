#include <iostream>
#include "SinglyList.h"
#include "DoublyList.h"
using namespace std;

int main() {
    cout << "Односвязный список:" << endl;
    SinglyList sList;
    sList.Add(1.1);
    sList.Add(2.2);
    sList.Add(3.3);
    sList.Print();

    cout << "\nДвусвязный список:" << endl;
    DoublyList dList;
    dList.Add(4.4);
    dList.Add(5.5);
    dList.Add(6.6);
    dList.PrintForward();

    return 0;
}

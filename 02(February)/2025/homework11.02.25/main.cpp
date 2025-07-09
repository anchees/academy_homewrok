#include <iostream>
#include "DoublyList.h"
using namespace std;

int main() {
    DoublyList<int> list1 = {1, 2, 3}; 
    cout << "list1: ";
    list1.PrintForward();

    DoublyList<int> list2 = std::move(list1);
    cout << "list2 (после перемещения): ";
    list2.PrintForward();

    DoublyList<int> list3;
    list3 = std::move(list2);
    cout << "list3 (после перемещающего присваивания): ";
    list3.PrintForward();

    return 0;
}

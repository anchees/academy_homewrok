#include "DynArray.h"
#include "DoubleList.h"

int main() {
    try {
        DynArray arr(5);
        arr[0] = 10;
        arr[4] = 20;
        cout << arr[0] << ", " << arr[4] << endl;

        cout << arr[5] << endl;
    }
    catch (const exception& e) {
        cout << "Ошибка: " << e.what() << endl;
    }


    try {
        DoubleList list;
        list.push_back(1);
        list.push_back(2);
        list.print_forward(); 

        list.pop_back();
        list.pop_back();

        list.pop_back();
    }
    catch (const exception& e) {
        cout << "Ошибка: " << e.what() << endl;
    }


    return 0;
}

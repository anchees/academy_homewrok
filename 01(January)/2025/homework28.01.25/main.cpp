#include "stack.h"

int main() {
    int n;
    cout << "Введите размер стека: ";
    cin >> n;

    Stack st(n);
    st.push(10);
    st.push(20);
    st.push(30);
    st.print();

    cout << "Верхний элемент: " << st.top() << endl;

    st.pop();
    st.print();

    cout << "Текущий размер: " << st.getCount() << " из " << st.getCapacity() << endl;

    return 0;
}

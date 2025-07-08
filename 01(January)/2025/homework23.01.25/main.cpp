#include "Matrix.h"
using namespace std;

int main() {
    Matrix<int, 3, 3> A, B, C;

    cout << "Матрица A (рандом):\n";
    A.fillRandom();
    A.print();

    cout << "\nМатрица B (ввод):\n";
    B.input();

    cout << "\nСумма A + B:\n";
    C = A + B;
    C.print();

    cout << "\nРазность A - B:\n";
    C = A - B;
    C.print();

    cout << "\nПоэлементное умножение A * B:\n";
    C = A * B;
    C.print();

    return 0;
}

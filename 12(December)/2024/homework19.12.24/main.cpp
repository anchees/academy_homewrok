#include "point.h"

int main() {
    Point p(10, 5);

    cout << "p       = " << p << endl;

    cout << "p + 3   = " << p + 3 << endl;
    cout << "p - 2   = " << p - 2 << endl;
    cout << "p * 2   = " << p * 2 << endl;
    cout << "p / 5   = " << p / 5 << endl;

    cout << "3 + p   = " << 3 + p << endl;
    cout << "7 - p   = " << 7 - p << endl;
    cout << "2 * p   = " << 2 * p << endl;
    cout << "100 / p = " << 100 / p << endl;

    return 0;
}
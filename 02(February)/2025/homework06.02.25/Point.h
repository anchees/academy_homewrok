#ifndef POINT_H
#define POINT_H

#include <iostream>
using namespace std;

template <typename T>
class Point {
public:
    T x, y;

    Point(T x = 0, T y = 0) : x(x), y(y) {}

    Point operator+(const Point& other) const {
        return Point(x + other.x, y + other.y);
    }

    Point operator+(T value) const {
        return Point(x + value, y + value);
    }

    void print() const {
        cout << "(" << x << ", " << y << ")" << endl;
    }
};

template <typename T>
Point<T> operator+(T value, const Point<T>& p) {
    return Point<T>(value + p.x, value + p.y);
}

#endif 

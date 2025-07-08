#ifndef POINT_H
#define POINT_H

#include <iostream>
using namespace std;

class Point {
private:
    int x, y;

public:
    Point(int x = 0, int y = 0);
    int getX() const;
    int getY() const;
    Point operator+(int value) const;
    Point operator-(int value) const;
    Point operator*(int value) const;
    Point operator/(int value) const;
    friend Point operator+(int value, const Point& p);
    friend Point operator-(int value, const Point& p);
    friend Point operator*(int value, const Point& p);
    friend Point operator/(int value, const Point& p);
    friend ostream& operator<<(ostream& os, const Point& p);
};
#endif

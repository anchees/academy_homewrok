#include "point.h"
#include <stdexcept>

Point::Point(int x, int y) : x(x), y(y) {}

int Point::getX() const { return x; }
int Point::getY() const { return y; }

Point Point::operator+(int value) const {
    return Point(x + value, y + value);
}

Point Point::operator-(int value) const {
    return Point(x - value, y - value);
}

Point Point::operator*(int value) const {
    return Point(x * value, y * value);
}

Point Point::operator/(int value) const {
    if (value == 0) {
        throw invalid_argument("Division by zero!");
    }
    return Point(x / value, y / value);
}

Point operator+(int value, const Point& p) {
    return Point(value + p.x, value + p.y);
}

Point operator-(int value, const Point& p) {
    return Point(value - p.x, value - p.y);
}

Point operator*(int value, const Point& p) {
    return Point(value * p.x, value * p.y);
}

Point operator/(int value, const Point& p) {
    if (p.x == 0 || p.y == 0) {
        throw invalid_argument("Division by zero in Point components!");
    }
    return Point(value / p.x, value / p.y);
}
ostream& operator<<(ostream& os, const Point& p) {
    os << "(" << p.x << ", " << p.y << ")";
    return os;
}

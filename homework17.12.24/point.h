#include <iostream>
using namespace std;

class point{
private:
    int x, y;
public:
    point(int xx = 0, int yy = 0) : x(xx), y(yy) {}
    point(point& other) : x(other.x), y(other.y) {} 


    point& operator=(point &other){
        x=other.x;
        y=other.y;
        return *this;
    }

    point& operator--() {
        --x;
        --y;
        return *this;
    }

    point operator--(int) {
        x--;
        y--;
        return *this;
    }

    void print() const {
        cout<<"x: "<<x<<" y: " <<y<<endl;
    }
};
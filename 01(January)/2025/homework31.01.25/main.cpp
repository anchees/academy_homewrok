#include <iostream>
#include "priority_queue.h"
using namespace std;

int main() {
    PriorityQueue pq(5);

    pq.push("Низкий", 1);
    pq.push("Средний", 5);
    pq.push("Высокий", 10);

    pq.print();

    while (!pq.isEmpty()) {
        cout << "Обрабатываем: " << pq.top() << endl;
        pq.pop();
    }

    return 0;
}

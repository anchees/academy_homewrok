#include <iostream>
#include <vector>
#include <iomanip>
using namespace std;

int main() {
    const int rows = 10;
    const int cols = 10;

    vector<vector<int>> table(rows, vector<int>(cols));
    
    for (int i = 0; i < rows; ++i)
        for (int j = 0; j < cols; ++j)
            table[i][j] = (i + 1) * (j + 1);

    for (const auto& row : table) {
        for (const auto& val : row)
            cout << setw(4) << val;
        cout << endl;
    }

    return 0;
}

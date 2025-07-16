#include <iostream>
#include <vector>
#include <algorithm>
#include <iomanip>
#include "Team.h"

using namespace std;

bool compare(const Team& a, const Team& b) {
    if (a.getPoints() != b.getPoints())
        return a.getPoints() > b.getPoints(); // по очкам
    else
        return a.getGoalDiff() > b.getGoalDiff(); // по разнице мячей
}

void printTable(const vector<Team>& teams) {
    cout << left << setw(17) << "Название команды"
         << "| " << "Победы | Поражения | Ничьи | Забитые | Пропущенные | Разница | Очки" << endl;
    cout << string(85, '-') << endl;

    for (const auto& t : teams) {
        cout << left << setw(17) << t.name
             << "| " << setw(7) << t.wins
             << "| " << setw(10) << t.losses
             << "| " << setw(6) << t.draws
             << "| " << setw(8) << t.goalsFor
             << "| " << setw(13) << t.goalsAgainst
             << "| " << setw(8) << t.getGoalDiff()
             << "| " << t.getPoints() << endl;
    }
}

void printResults(const vector<Team>& teams) {
    cout << "\nПобедитель чемпионата - " << teams[0].name << endl;
    cout << teams[0].name << "   1 место\n";
    cout << teams[1].name << "   2 место\n";
    cout << teams[2].name << "   3 место\n";

    cout << "\n*Стыковые матчи\n";
    cout << teams[teams.size() - 4].name << "   4е место с конца\n";
    cout << teams[teams.size() - 3].name << "   3е место с конца\n";

    cout << "\nВылет во 2-й дивизион\n";
    cout << teams[teams.size() - 2].name << "   Предпоследнее место\n";
    cout << teams[teams.size() - 1].name << "   Последнее место\n";
}

int main() {
    vector<Team> teams = {
        {"Спартак", 20, 4, 6, 32, 18},
        {"Динамо М.", 10, 15, 5, 37, 55},
        {"Химки", 18, 8, 4, 28, 24},
        {"ЦСКА М.", 17, 9, 4, 30, 20},
        {"Ахмат", 14, 10, 6, 25, 23},
        {"Урал", 13, 12, 5, 21, 22},
        {"Торпедо М.", 6, 20, 4, 18, 42}
    };

    sort(teams.begin(), teams.end(), compare);
    printTable(teams);
    printResults(teams);

    return 0;
}


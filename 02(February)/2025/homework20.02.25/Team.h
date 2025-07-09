#ifndef TEAM_H
#define TEAM_H

#include <string>
using namespace std;

struct Team {
    string name;
    int wins, losses, draws;
    int goalsFor, goalsAgainst;

    Team(string n = "", int w = 0, int l = 0, int d = 0, int gf = 0, int ga = 0)
        : name(n), wins(w), losses(l), draws(d), goalsFor(gf), goalsAgainst(ga) {}

    int getPoints() const { return wins * 3 + draws; }
    int getGoalDiff() const { return goalsFor - goalsAgainst; }
};

#endif

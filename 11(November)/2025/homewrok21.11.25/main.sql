-- Все возможные пары преподавателей и групп
SELECT
    T.Name,
    T.Surname,
    G.Name AS GroupName
FROM Teachers T
CROSS JOIN Groups G;


-- Факультеты, у которых фонд кафедр больше фонда факультета
SELECT
    F.Name,
    SUM(D.Financing) AS DepartmentsFinancing,
    F.Financing AS FacultyFinancing
FROM Faculties F
JOIN Departments D ON D.FacultyId = F.Id
GROUP BY F.Id, F.Name, F.Financing
HAVING SUM(D.Financing) > F.Financing;

-- Фамилии кураторов и названия групп, которые они курируют
SELECT
    C.Surname,
    G.Name AS GroupName
FROM Curators C
JOIN GroupsCurators GC ON GC.CuratorId = C.Id
JOIN Groups G ON G.Id = GC.GroupId;

-- Преподаватели, читающие лекции у группы P107
SELECT
    T.Name,
    T.Surname
FROM Teachers T
JOIN Lectures L ON L.TeacherId = T.Id
JOIN GroupsLectures GL ON GL.LectureId = L.Id
JOIN Groups G ON G.Id = GL.GroupId
WHERE G.Name = 'P107';

-- Преподаватели и факультеты, на которых они читают лекции
SELECT DISTINCT
    T.Surname,
    F.Name AS FacultyName
FROM Teachers T
JOIN Lectures L ON L.TeacherId = T.Id
JOIN GroupsLectures GL ON GL.LectureId = L.Id
JOIN Groups G ON G.Id = GL.GroupId
JOIN Departments D ON D.Id = G.DepartmentId
JOIN Faculties F ON F.Id = D.FacultyId;

-- Кафедры и группы, которые к ним относятся
SELECT
    D.Name AS DepartmentName,
    G.Name AS GroupName
FROM Departments D
JOIN Groups G ON G.DepartmentId = D.Id;

-- Дисциплины, которые читает Samantha Adams
SELECT DISTINCT
    S.Name
FROM Subjects S
JOIN Lectures L ON L.SubjectId = S.Id
JOIN Teachers T ON T.Id = L.TeacherId
WHERE T.Name = 'Samantha'
  AND T.Surname = 'Adams';

-- Кафедры, на которых читается дисциплина Database Theory
SELECT DISTINCT
    D.Name
FROM Departments D
JOIN Groups G ON G.DepartmentId = D.Id
JOIN GroupsLectures GL ON GL.GroupId = G.Id
JOIN Lectures L ON L.Id = GL.LectureId
JOIN Subjects S ON S.Id = L.SubjectId
WHERE S.Name = 'Database Theory';

-- Группы факультета Computer Science
SELECT
    G.Name
FROM Groups G
JOIN Departments D ON D.Id = G.DepartmentId
JOIN Faculties F ON F.Id = D.FacultyId
WHERE F.Name = 'Computer Science';

-- Группы 5-го курса и их факультеты
SELECT
    G.Name AS GroupName,
    F.Name AS FacultyName
FROM Groups G
JOIN Departments D ON D.Id = G.DepartmentId
JOIN Faculties F ON F.Id = D.FacultyId
WHERE G.Year = 5;

-- Преподаватели и лекции в аудитории B103
SELECT
    T.Name + ' ' + T.Surname AS Teacher,
    S.Name AS Subject,
    G.Name AS GroupName
FROM Teachers T
JOIN Lectures L ON L.TeacherId = T.Id
JOIN Subjects S ON S.Id = L.SubjectId
JOIN GroupsLectures GL ON GL.LectureId = L.Id
JOIN Groups G ON G.Id = GL.GroupId
WHERE L.LectureRoom = 'B103';

-- Задание 1
--Вернуть информацию о барбере, который работает в барбершопе дольше всех
CREATE PROCEDURE dbo.usp_GetLongestWorkingBarber
AS
BEGIN
    SET NOCOUNT ON;

    SELECT TOP 1 *
    FROM Barbers
    WHERE IsActive = 1
    ORDER BY HireDate ASC;
END;
GO

-- Вернуть информацию о барбере, который обслужил максимальное количество клиентов в указанном диапазонедат. 
-- Даты передаются в качестве параметра
CREATE PROCEDURE dbo.usp_GetTopBarberByClientsInPeriod
    @DateFrom DATE,
    @DateTo   DATE
AS
BEGIN
    SET NOCOUNT ON;

    SELECT TOP 1
        b.BarberID,
        b.FullName,
        b.HireDate,
        b.BarberLevel,
        COUNT(DISTINCT a.ClientID) AS ClientsCount
    FROM Appointments a
    JOIN Barbers b ON b.BarberID = a.BarberID
    WHERE a.Status = 'Completed'
      AND CAST(a.StartDateTime AS DATE) BETWEEN @DateFrom AND @DateTo
    GROUP BY b.BarberID, b.FullName, b.HireDate, b.BarberLevel
    ORDER BY ClientsCount DESC;
END;
GO

-- Вернуть информацию о клиенте, который посетил барбершоп максимальное количество раз
CREATE PROCEDURE dbo.usp_GetMostFrequentClient
AS
BEGIN
    SET NOCOUNT ON;

    SELECT TOP 1
        c.ClientID,
        c.FullName,
        c.Phone,
        COUNT(a.AppointmentID) AS VisitCount
    FROM Clients c
    JOIN Appointments a ON a.ClientID = c.ClientID
    WHERE a.Status = 'Completed'
    GROUP BY c.ClientID, c.FullName, c.Phone
    ORDER BY VisitCount DESC;
END;
GO

-- Вернуть информацию о клиенте, который потратил наибольшую сумму денег в барбершопе
CREATE PROCEDURE dbo.usp_GetTopSpendingClient
AS
BEGIN
    SET NOCOUNT ON;

    SELECT TOP 1
        c.ClientID,
        c.FullName,
        c.Phone,
        SUM(a.TotalPrice) AS TotalSpent
    FROM Clients c
    JOIN Appointments a ON a.ClientID = c.ClientID
    WHERE a.Status = 'Completed'
    GROUP BY c.ClientID, c.FullName, c.Phone
    ORDER BY TotalSpent DESC;
END;
GO

-- Вернуть информацию о самой длинной по времени услугев барбершопе
CREATE PROCEDURE dbo.usp_GetLongestService
AS
BEGIN
    SET NOCOUNT ON;

    SELECT TOP 1 *
    FROM Services
    ORDER BY DurationMin DESC;
END;
GO


-- Задание 2
-- Вернуть информацию о самом популярном барбере (по количеству клиентов)
CREATE PROCEDURE dbo.usp_GetMostPopularBarber
AS
BEGIN
    SET NOCOUNT ON;

    SELECT TOP 1
        b.BarberID,
        b.FullName,
        b.HireDate,
        b.BarberLevel,
        COUNT(DISTINCT a.ClientID) AS ClientsCount
    FROM Barbers b
    JOIN Appointments a ON a.BarberID = b.BarberID
    WHERE a.Status = 'Completed'
    GROUP BY b.BarberID, b.FullName, b.HireDate, b.BarberLevel
    ORDER BY ClientsCount DESC;
END;
GO

-- Вернуть топ-3 барберов за месяц (по сумме денег, потраченной клиентами)
CREATE PROCEDURE dbo.usp_GetTop3BarbersByRevenueForMonth
    @Year  INT,
    @Month INT
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @DateFrom DATE = dbo.fn_GetMonthStart(@Year, @Month);
    DECLARE @DateTo   DATE = dbo.fn_GetMonthEnd(@Year, @Month);

    SELECT TOP 3
        b.BarberID,
        b.FullName,
        SUM(a.TotalPrice) AS Revenue
    FROM Barbers b
    JOIN Appointments a ON a.BarberID = b.BarberID
    WHERE a.Status = 'Completed'
      AND CAST(a.StartDateTime AS DATE) BETWEEN @DateFrom AND @DateTo
    GROUP BY b.BarberID, b.FullName
    ORDER BY Revenue DESC;
END;
GO

-- Вернуть топ-3 барберов за всё время (по средней оценке). Количество посещений клиентов не меньше 30
CREATE PROCEDURE dbo.usp_GetTop3BarbersByRatingAllTime
AS
BEGIN
    SET NOCOUNT ON;

    SELECT TOP 3
        b.BarberID,
        b.FullName,
        COUNT(a.AppointmentID) AS VisitsCount,
        AVG(CAST(a.Rating AS DECIMAL(10,2))) AS AvgRating
    FROM Barbers b
    JOIN Appointments a ON a.BarberID = b.BarberID
    WHERE a.Status = 'Completed'
      AND a.Rating IS NOT NULL
    GROUP BY b.BarberID, b.FullName
    HAVING COUNT(a.AppointmentID) >= 30
    ORDER BY AvgRating DESC, VisitsCount DESC;
END;
GO

-- Показать расписание на день конкретного барбера. Информация о барбере и дне передаётся в качестве параметра
CREATE PROCEDURE dbo.usp_GetBarberScheduleForDay
    @BarberID INT,
    @Day DATE
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        a.AppointmentID,
        c.FullName AS ClientName,
        s.ServiceName,
        a.StartDateTime,
        a.EndDateTime,
        a.Status
    FROM Appointments a
    JOIN Clients c ON c.ClientID = a.ClientID
    JOIN Services s ON s.ServiceID = a.ServiceID
    WHERE a.BarberID = @BarberID
      AND CAST(a.StartDateTime AS DATE) = @Day
      AND a.Status <> 'Cancelled'
    ORDER BY a.StartDateTime;
END;
GO

-- Показать свободные временные слоты на неделю конкретного барбера. Информация о барбере и дне передаётсяв качестве параметра
CREATE PROCEDURE dbo.usp_GetFreeSlotsForWeek
    @BarberID INT,
    @StartDate DATE
AS
BEGIN
    SET NOCOUNT ON;

    SELECT *
    FROM dbo.fn_GetFreeSlotsForWeek(@BarberID, @StartDate)
    ORDER BY SlotStart;
END;
GO

-- Перенести в архив информацию о всех уже завершенных услугах (это те услуги, которые произошли в прошлом)
CREATE PROCEDURE dbo.usp_ArchivePastAppointments
AS
BEGIN
    SET NOCOUNT ON;

    INSERT INTO AppointmentsArchive
    (
        AppointmentID,
        ClientID,
        BarberID,
        ServiceID,
        StartDateTime,
        EndDateTime,
        TotalPrice,
        Status,
        Feedback,
        Rating
    )
    SELECT
        a.AppointmentID,
        a.ClientID,
        a.BarberID,
        a.ServiceID,
        a.StartDateTime,
        a.EndDateTime,
        a.TotalPrice,
        a.Status,
        a.Feedback,
        a.Rating
    FROM Appointments a
    WHERE a.EndDateTime < GETDATE()
      AND a.Status = 'Completed'
      AND NOT EXISTS
      (
          SELECT 1
          FROM AppointmentsArchive ar
          WHERE ar.AppointmentID = a.AppointmentID
      );

    DELETE FROM Appointments
    WHERE EndDateTime < GETDATE()
      AND Status = 'Completed';
END;
GO

-- Запретить записывать клиента к барберу на уже занятое время и дату
CREATE TRIGGER trg_PreventOverlappingAppointments
ON Appointments
INSTEAD OF INSERT, UPDATE
AS
BEGIN
    SET NOCOUNT ON;
    IF EXISTS
    (
        SELECT 1
        FROM inserted
        WHERE StartDateTime >= EndDateTime
    )
    BEGIN
        RAISERROR('Некорректный интервал времени записи.', 16, 1);
        RETURN;
    END;
    IF EXISTS
    (
        SELECT 1
        FROM inserted i
        JOIN Appointments a
            ON a.BarberID = i.BarberID
           AND a.AppointmentID <> ISNULL(i.AppointmentID, -1)
           AND a.Status <> 'Cancelled'
           AND i.Status <> 'Cancelled'
           AND i.StartDateTime < a.EndDateTime
           AND i.EndDateTime > a.StartDateTime
    )
    BEGIN
        RAISERROR('Нельзя записать клиента: у барбера уже занято это время.', 16, 1);
        RETURN;
    END;
    IF EXISTS
    (
        SELECT 1
        FROM deleted
    )
    BEGIN
        UPDATE a
        SET
            a.ClientID = i.ClientID,
            a.BarberID = i.BarberID,
            a.ServiceID = i.ServiceID,
            a.StartDateTime = i.StartDateTime,
            a.EndDateTime = i.EndDateTime,
            a.TotalPrice = i.TotalPrice,
            a.Status = i.Status,
            a.Feedback = i.Feedback,
            a.Rating = i.Rating
        FROM Appointments a
        JOIN inserted i ON a.AppointmentID = i.AppointmentID;
    END
    ELSE
    BEGIN
        INSERT INTO Appointments
        (
            ClientID,
            BarberID,
            ServiceID,
            StartDateTime,
            EndDateTime,
            TotalPrice,
            Status,
            Feedback,
            Rating
        )
        SELECT
            ClientID,
            BarberID,
            ServiceID,
            StartDateTime,
            EndDateTime,
            TotalPrice,
            Status,
            Feedback,
            Rating
        FROM inserted;
    END
END;
GO

-- Запретить добавление нового джуниор-барбера, если в салоне уже работают 5 джуниор-барберов
CREATE TRIGGER trg_LimitJuniorBarbers
ON Barbers
INSTEAD OF INSERT
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @CurrentJuniorCount INT;
    DECLARE @NewJuniorCount INT;

    SELECT @CurrentJuniorCount = COUNT(*)
    FROM Barbers
    WHERE BarberLevel = 'Junior'
      AND IsActive = 1;

    SELECT @NewJuniorCount = COUNT(*)
    FROM inserted
    WHERE BarberLevel = 'Junior'
      AND IsActive = 1;

    IF @CurrentJuniorCount + @NewJuniorCount > 5
    BEGIN
        RAISERROR('Нельзя добавить нового junior-барбера: в салоне уже работают 5 junior-барберов.', 16, 1);
        RETURN;
    END;

    INSERT INTO Barbers (FullName, HireDate, BarberLevel, IsActive)
    SELECT FullName, HireDate, BarberLevel, IsActive
    FROM inserted;
END;
GO

-- Вернуть информацию о клиентах, которые не поставилини одного фидбека и ни одной оценки
CREATE PROCEDURE dbo.usp_GetClientsWithoutFeedbackAndRating
AS
BEGIN
    SET NOCOUNT ON;

    SELECT c.*
    FROM Clients c
    WHERE EXISTS
    (
        SELECT 1
        FROM Appointments a
        WHERE a.ClientID = c.ClientID
          AND a.Status = 'Completed'
    )
    AND NOT EXISTS
    (
        SELECT 1
        FROM Appointments a
        WHERE a.ClientID = c.ClientID
          AND (
                a.Feedback IS NOT NULL
                OR a.Rating IS NOT NULL
              )
    );
END;
GO

-- Вернуть информацию о клиентах, которые не посещалибарбершоп свыше одного года
CREATE PROCEDURE dbo.usp_GetClientsAbsentMoreThanYear
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        c.ClientID,
        c.FullName,
        c.Phone,
        MAX(a.StartDateTime) AS LastVisit
    FROM Clients c
    LEFT JOIN Appointments a
        ON a.ClientID = c.ClientID
       AND a.Status = 'Completed'
    GROUP BY c.ClientID, c.FullName, c.Phone
    HAVING MAX(a.StartDateTime) IS NULL
        OR MAX(a.StartDateTime) < DATEADD(YEAR, -1, GETDATE());
END;
GO
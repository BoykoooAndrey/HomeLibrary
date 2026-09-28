#!/bin/bash
set -e

SQLCMD="/opt/mssql-tools/bin/sqlcmd"

echo "Waiting for SQL Server..."

for i in {1..30}
do
    if "$SQLCMD" \
        -S mssql \
        -U sa \
        -P "$MSSQL_SA_PASSWORD" \
        -Q "SELECT 1" > /dev/null 2>&1
    then
        echo "SQL Server is ready."
        break
    fi

    if [ "$i" -eq 30 ]; then
        echo "SQL Server did not become ready in time."
        exit 1
    fi

    sleep 2
done

echo "Waiting for SQL Server startup to complete..."
sleep 10
echo "Executing /scripts/01_CreateDatabase.sql..."

"$SQLCMD" \
    -S mssql \
    -U sa \
    -P "$MSSQL_SA_PASSWORD" \
    -b \
    -i "/scripts/01_CreateDatabase.sql"

echo "Waiting for HomeLibrary database..."

for i in {1..30}
do
    STATUS=$("$SQLCMD" \
        -S mssql \
        -U sa \
        -P "$MSSQL_SA_PASSWORD" \
        -h -1 \
        -W \
        -Q "SET NOCOUNT ON; SELECT state_desc FROM sys.databases WHERE name = 'HomeLibrary'" \
        2>/dev/null || true)

    if echo "$STATUS" | grep -q "ONLINE"; then
        echo "HomeLibrary database is ready."
        break
    fi

    if [ "$i" -eq 30 ]; then
        echo "HomeLibrary database did not become ready in time."
        exit 1
    fi

    sleep 2
done

for file in /scripts/*.sql
do
    if [ "$file" = "/scripts/01_CreateDatabase.sql" ]; then
        continue
    fi

    echo "Executing $file..."

    "$SQLCMD" \
        -S mssql \
        -U sa \
        -P "$MSSQL_SA_PASSWORD" \
        -b \
        -i "$file"
done

echo "Database initialization completed."
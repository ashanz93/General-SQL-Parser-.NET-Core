using System;
using System.Collections.Generic;
using System.Text;

namespace GeneralSQLParserPOC
{
    internal static class Queries
    {
        internal static string SelectQuery = @"SELECT e.last_name AS name, e.commission_pct comm, e.salary * 12 ""Annual Salary"" FROM scott.employees AS e WHERE e.salary > 1000 ORDER BY e.first_name, e.last_name;";
        internal static string UpdateQuery = @"UPDATE customers SET state = 'California', customer_rep = 32 WHERE customer_id > 100;";
        internal static string CreateTableQuery = @"CREATE TABLE departments_demo
    ( department_id    NUMBER(4)
    , department_name  VARCHAR2(30)
           CONSTRAINT  dept_name_nn  NOT NULL
    , manager_id       NUMBER(6)
    , location_id      NUMBER(4)
    , dn               VARCHAR2(300)
    ) ;";
        internal static string CreateTableQuery2 = @"CREATE TABLE departments_demo
    ( department_id    NUMBER(4)   PRIMARY KEY DISABLE
    , department_name  VARCHAR2(30)
           CONSTRAINT  dept_name_nn  NOT NULL
    , manager_id       NUMBER(6)
    , location_id      NUMBER(4)
    , dn               VARCHAR2(300)
    ) ;";
        internal static string CreateViewQuery = @"CREATE VIEW sup_orders AS
  SELECT suppliers.supplier_id, orders.quantity, orders.price
  FROM suppliers
  INNER JOIN orders
  ON suppliers.supplier_id = orders.supplier_id
  WHERE suppliers.supplier_name = 'Microsoft';
	 ";
    }
}

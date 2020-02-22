using gudusoft.gsqlparser;
using gudusoft.gsqlparser.nodes;
using gudusoft.gsqlparser.stmt;
using System;
using System.Collections.Generic;
using System.Text;

namespace GeneralSQLParserPOC.MetadataAnalysis
{
    public class MetadataAnalysis
    {
        public static void AnalyzeStmt(TCustomSqlStatement stmt)
        {
            switch (stmt.sqlstatementtype)
            {
                case ESqlStatementType.sstselect:
                    AnalyzeSelectStmt((TSelectSqlStatement)stmt);
                    break;
                case ESqlStatementType.sstupdate:
                    AnalyzeUpdateStmt((TUpdateSqlStatement)stmt);
                    break;
                case ESqlStatementType.sstcreatetable:
                    AnalyzeCreateTableStmt((TCreateTableSqlStatement)stmt);
                    break;
                case ESqlStatementType.sstaltertable:
                    break;
                case ESqlStatementType.sstcreateview:
                    AnalyzeCreateViewStmt((TCreateViewSqlStatement)stmt);
                    break;
                default:
                    Console.WriteLine(stmt.sqlstatementtype.ToString());
                    break;
            }
        }

        private static void AnalyzeSelectStmt(TSelectSqlStatement stmt)
        {
            Console.WriteLine("Select Statement");
            if(stmt.CombinedQuery)
            {
                string setstr = string.Empty;
                switch(stmt.SetOperator)
                {
                    case TSelectSqlStatement.setOperator_union:
                        setstr = "union";
                        break;
                    case TSelectSqlStatement.setOperator_unionall:
                        setstr = "union all";
                        break;
                    case TSelectSqlStatement.setOperator_except:
                        setstr = "except";
                        break;
                    case TSelectSqlStatement.setOperator_exceptall:
                        setstr = "except all";
                        break;
                    case TSelectSqlStatement.setOperator_minus:
                        setstr = "minus";
                        break;
                    case TSelectSqlStatement.setOperator_minusall:
                        setstr = "minus all";
                        break;
                    case TSelectSqlStatement.setOperator_intersect:
                        setstr = "intersect";
                        break;
                    case TSelectSqlStatement.setOperator_intersectall:
                        setstr = "intersect all";
                        break;
                }
                Console.WriteLine(string.Format("set type: {0}", setstr));
                Console.WriteLine("left select: ");
                AnalyzeSelectStmt(stmt.LeftStmt);
                Console.WriteLine("left select: ");
                AnalyzeSelectStmt(stmt.RightStmt);
                if(stmt.OrderbyClause != null)
                {
                    if(!string.IsNullOrEmpty(stmt.OrderbyClause.ToScript()))
                    {
                        Console.WriteLine("Order by Clause: " + stmt.OrderbyClause.ToString());
                    }
                }
            }
            else
            {
                // select list
                for(int i = 0; i < stmt.ResultColumnList.Count; i++)
                {
                    TResultColumn resultColumn = stmt.ResultColumnList.getResultColumn(i);
                    Console.WriteLine(string.Format("Column: {0}, Alias: {1}", resultColumn.Expr.ToString(), (resultColumn.AliasClause == null) ? string.Empty : resultColumn.AliasClause.ToString()));
                }

                // from clause
                for(int i = 0; i < stmt.joins.Count; i++)
                {
                    TJoin join = stmt.joins.getJoin(i);
                    switch(join.Kind)
                    {
                        case TBaseType.join_source_fake:
                            Console.WriteLine(string.Format("table: {0},  alias: {1}", join.Table.ToString(), (join.Table.AliasClause == null) ? string.Empty : join.Table.AliasClause.ToString()));
                            break;
                        case TBaseType.join_source_table:
                            Console.WriteLine(string.Format("table: {0},  alias: {1}", join.Table.ToString(), (join.Table.AliasClause == null) ? string.Empty : join.Table.AliasClause.ToString()));
                            for(int j = 0; j < join.JoinItems.Count; j++)
                            {
                                TJoinItem joinItem = join.JoinItems.getJoinItem(j);
                                Console.WriteLine(string.Format("Join type: {0}", joinItem.JoinType.ToString()));
                                Console.WriteLine(string.Format("table: {0},  alias: {1}", joinItem.Table.ToString(), (joinItem.Table.AliasClause == null) ? string.Empty : joinItem.Table.AliasClause.ToString()));
                                if(joinItem.OnCondition != null)
                                {
                                    Console.WriteLine(string.Format("On: {0}", joinItem.OnCondition.ToString()));
                                }
                                else if(joinItem.UsingColumns != null)
                                {
                                    Console.WriteLine(string.Format("Using: {0}", joinItem.UsingColumns.ToString()));
                                }
                            }
                            break;
                        case TBaseType.join_source_join:
                            TJoin source_join = join.Join;
                            Console.WriteLine(string.Format("table: {0},  alias: {1}", source_join.Table.ToString(), (source_join.Table.AliasClause == null) ? string.Empty : source_join.Table.AliasClause.ToString()));
                            for (int j = 0; j < source_join.JoinItems.Count; j++)
                            {
                                TJoinItem joinItem = source_join.JoinItems.getJoinItem(j);
                                Console.WriteLine(string.Format("source_join type: {0}", joinItem.JoinType.ToString()));
                                Console.WriteLine(string.Format("table: {0},  alias: {1}", joinItem.Table.ToString(), (joinItem.Table.AliasClause == null) ? string.Empty : joinItem.Table.AliasClause.ToString()));
                                if (joinItem.OnCondition != null)
                                {
                                    Console.WriteLine(string.Format("On: {0}", joinItem.OnCondition.ToString()));
                                }
                                else if (joinItem.UsingColumns != null)
                                {
                                    Console.WriteLine(string.Format("Using: {0}", joinItem.UsingColumns.ToString()));
                                }
                            }

                            for (int j = 0; j < join.JoinItems.Count; j++)
                            {
                                TJoinItem joinItem = join.JoinItems.getJoinItem(j);
                                Console.WriteLine(string.Format("Join type: {0}", joinItem.JoinType.ToString()));
                                Console.WriteLine(string.Format("table: {0},  alias: {1}", joinItem.Table.ToString(), (joinItem.Table.AliasClause == null) ? string.Empty : joinItem.Table.AliasClause.ToString()));
                                if (joinItem.OnCondition != null)
                                {
                                    Console.WriteLine(string.Format("On: {0}", joinItem.OnCondition.ToString()));
                                }
                                else if (joinItem.UsingColumns != null)
                                {
                                    Console.WriteLine(string.Format("Using: {0}", joinItem.UsingColumns.ToString()));
                                }
                            }

                            break;
                        default:
                            break;
                    }
                }

                // where clause
                if(stmt.WhereClause != null)
                {
                    if(!string.IsNullOrEmpty(stmt.WhereClause.ToString()))
                    {
                        Console.WriteLine("Where clause: " + stmt.WhereClause.Condition.ToString());
                    }
                }

                // group by 
                if (stmt.GroupByClause != null)
                {
                    if (!string.IsNullOrEmpty(stmt.GroupByClause.ToString()))
                    {
                        Console.WriteLine("group by clause: " + stmt.GroupByClause.ToString());
                    }
                }

                // order by
                if (stmt.OrderbyClause != null)
                {
                    Console.WriteLine("Order by:");
                    for(int i = 0; i < stmt.OrderbyClause.Items.Count; i++)
                    {
                        Console.WriteLine(string.Format("\t{0}", stmt.OrderbyClause.Items.getOrderByItem(i).ToString()));
                    }
                }

                // for update 
                if (stmt.ForUpdateClause != null)
                {
                    if (!string.IsNullOrEmpty(stmt.ForUpdateClause.ToString()))
                    {
                        Console.WriteLine("for update clause: " + stmt.ForUpdateClause.ToString());
                    }
                }

                // top clause
                if (stmt.TopClause != null)
                {
                    if (!string.IsNullOrEmpty(stmt.TopClause.ToString()))
                    {
                        Console.WriteLine("top clause: " + stmt.TopClause.ToString());
                    }
                }

                // limit clause
                if (stmt.LimitClause != null)
                {
                    if (!string.IsNullOrEmpty(stmt.LimitClause.ToString()))
                    {
                        Console.WriteLine("limit clause: " + stmt.LimitClause.ToString());
                    }
                }
            }
        }

        private static void AnalyzeUpdateStmt(TUpdateSqlStatement stmt)
        {
            Console.WriteLine(string.Format("Table name: {0}", stmt.TargetTable.ToString()));
            Console.WriteLine("set clause:");
            for(int i = 0; i < stmt.ResultColumnList.Count; i++)
            {
                TResultColumn resultColumn = stmt.ResultColumnList.getResultColumn(i);
                TExpression expression = resultColumn.Expr;
                Console.WriteLine(string.Format("\tColumn: {0}\tvalue: {1}", expression.LeftOperand.ToString(), expression.RightOperand.ToString()));
            }
            if(stmt.WhereClause != null)
            {
                Console.WriteLine("Where clause: {0}", stmt.WhereClause.Condition.ToString());
            }
        }

        private static void AnalyzeCreateTableStmt(TCreateTableSqlStatement stmt)
        {
            Console.WriteLine(string.Format("Table name: {0}", stmt.TargetTable.ToString()));
            Console.WriteLine("Columns:");
            TColumnDefinition column;
            for(int i = 0; i < stmt.ColumnList.Count; i++)
            {
                column = stmt.ColumnList.getColumn(i);
                Console.WriteLine("\tname: {0}", column.ColumnName.ToString());
                Console.WriteLine("\tdatatype: {0}", column.Datatype.ToString());
                if(column.DefaultExpression != null)
                {
                    Console.WriteLine("\tdefault: {0}", column.DefaultExpression.ToString());
                }
                if(column.Null)
                {
                    Console.WriteLine("\tnull: yes");
                }
                if(column.Constraints != null)
                {
                    Console.WriteLine("\tinline constraints:");
                    for(int j = 0; j < column.Constraints.Count; j++)
                    {
                        PrintConstraint(column.Constraints.getConstraint(j), true);
                    }
                }
                Console.WriteLine();
            }
        }

        private static void PrintConstraint(TConstraint constraint, bool outline)
        {
            if(constraint.ConstraintName != null)
            {
                Console.WriteLine("\t\tconstraint name: {0}", constraint.ConstraintName.ToString());
            }

            switch(constraint.Constraint_type)
            {
                case EConstraintType.notnull:
                    Console.WriteLine("\t\tnot null");
                    break;
                case EConstraintType.primary_key:
                    Console.WriteLine("\t\tprimary key");
                    if(outline)
                    {
                        string setstr = string.Empty;
                        if(constraint.ColumnList != null)
                        {
                            for(int i = 0; i < constraint.ColumnList.Count; i++)
                            {
                                if(i != 0)
                                {
                                    setstr += ",";
                                }
                                setstr += constraint.ColumnList.getObjectName(i).ToString();
                            }
                            Console.WriteLine("\t\tprimary key columns: {0}", setstr);
                        }
                    }
                    break;
                case EConstraintType.unique:
                    Console.WriteLine("\t\tunique key");
                    if (outline)
                    {
                        string setstr = string.Empty;
                        if (constraint.ColumnList != null)
                        {
                            for (int i = 0; i < constraint.ColumnList.Count; i++)
                            {
                                if (i != 0)
                                {
                                    setstr += ",";
                                }
                                setstr += constraint.ColumnList.getObjectName(i).ToString();
                            }
                            Console.WriteLine("\t\tcolumns: {0}", setstr);
                        }
                    }
                    break;
                case EConstraintType.check:
                    Console.WriteLine("\t\tcheck: {0}", constraint.CheckCondition.ToString());
                    break;
                case EConstraintType.foreign_key:
                case EConstraintType.reference:
                    Console.WriteLine("\t\tforeign key");
                    if (outline)
                    {
                        string setstr = string.Empty;
                        if (constraint.ColumnList != null)
                        {
                            for (int i = 0; i < constraint.ColumnList.Count; i++)
                            {
                                if (i != 0)
                                {
                                    setstr += ",";
                                }
                                setstr += constraint.ColumnList.getObjectName(i).ToString();
                            }
                            Console.WriteLine("\t\tcolumns: {0}", setstr);
                        }
                    }
                    Console.WriteLine("\t\treferenced table: {0}", constraint.ReferencedObject.ToString());
                    if(constraint.ReferencedColumnList != null)
                    {
                        string setstr = string.Empty;
                        for (int i = 0; i < constraint.ReferencedColumnList.Count; i++)
                        {
                            if (i != 0)
                            {
                                setstr += ",";
                            }
                            setstr += constraint.ReferencedColumnList.getObjectName(i).ToString();
                        }
                        Console.WriteLine("\t\treferenced columns: {0}", setstr);
                    }
                    break;
                default:
                    break;
            }
        }

        private static void AnalyzeCreateViewStmt(TCreateViewSqlStatement stmt)
        {
            TCreateViewSqlStatement createView = stmt;
            Console.WriteLine("View name: {0}", createView.ViewName.ToString());
            TViewAliasClause aliasClause = createView.ViewAliasClause;
            if(aliasClause != null)
            {
                for (int i = 0; i < aliasClause.ViewAliasItemList.Count; i++)
                {
                    Console.WriteLine("View alias: {0}", aliasClause.ViewAliasItemList.getViewAliasItem(i).ToString());
                }
            }
            Console.WriteLine("View subquery: {0}", createView.Subquery.ToString());
            AnalyzeStmt(createView.Subquery);
        }
    }
}

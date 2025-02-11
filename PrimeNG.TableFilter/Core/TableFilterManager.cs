using System.Collections.Generic;
using System.Linq;
using PrimeNG.TableFilter.Models;
using PrimeNG.TableFilter.Utils;

namespace PrimeNG.TableFilter.Core
{
    public class TableFilterManager<TEntity> : ITableFilterManager<TEntity>
    {
        private const string FilterTypeMatchModeStartsWith = "startsWith";
        private const string FilterTypeMatchModeContains = "contains";
        private const string FilterTypeMatchModeNotContains = "notContains";
        private const string FilterTypeMatchModeEndsWith = "endsWith";
        private const string FilterTypeMatchModeEquals = "equals";
        private const string FilterTypeMatchModeNotEquals = "notEquals";
        private const string FilterTypeMatchModeIn = "in";
        private const string FilterTypeMatchModeLessThan = "lt";
        private const string FilterTypeMatchModeLessOrEqualsThan = "lte";
        private const string FilterTypeMatchModeGreaterThan = "gt";
        private const string FilterTypeMatchModeGreaterOrEqualsThan = "gte";
        private const string FilterTypeMatchModeBetween = "between";
        private const string FilterTypeMatchModeIs = "is";
        private const string FilterTypeMatchModeIsNot = "isNot";
        private const string FilterTypeMatchModeBefore = "before";
        private const string FilterTypeMatchModeAfter = "after";
        private const string FilterTypeMatchModeDateIs = "dateIs";
        private const string FilterTypeMatchModeDateIsNot = "dateIsNot";
        private const string FilterTypeMatchModeDateBefore = "dateBefore";
        private const string FilterTypeMatchModeDateAfter = "dateAfter";


        private readonly ILinqOperator<TEntity> _linqOperator;

        public TableFilterManager(IQueryable<TEntity> dataSet)
        {
            _linqOperator = new LinqOperator<TEntity>(dataSet);
        }

        public void MultipleOrderDataSet(TableFilterModel tableFilterPayload)
        {
            tableFilterPayload.MultiSortMeta.Select((value, i) => new {i, value}).ToList().ForEach(o =>
            {
                switch (o.value.Order)
                {
                    case (int) SortingEnumeration.OrderByAsc:
                        if (o.i == 0)
                            _linqOperator.OrderBy(o.value.Field.FirstCharToUpper());
                        else
                            _linqOperator.ThenBy(o.value.Field.FirstCharToUpper());
                        break;

                    case (int) SortingEnumeration.OrderByDesc:
                        if (o.i == 0)
                            _linqOperator.OrderByDescending(o.value.Field.FirstCharToUpper());
                        else
                            _linqOperator.ThenByDescending(o.value.Field.FirstCharToUpper());
                        break;

                    default:
                        throw new System.ArgumentException("Invalid Sort Order!");
                }
            });
        }

        public void SingleOrderDataSet(TableFilterModel tableFilterPayload)
        {
            switch (tableFilterPayload.SortOrder)
            {
                case (int) SortingEnumeration.OrderByAsc:
                    _linqOperator.OrderBy(tableFilterPayload.SortField.FirstCharToUpper());
                    break;

                case (int) SortingEnumeration.OrderByDesc:
                    _linqOperator.OrderByDescending(tableFilterPayload.SortField.FirstCharToUpper());
                    break;

                default:
                    throw new System.ArgumentException("Invalid Sort Order!");
            }
        }

        public void FilterDataSet(string key, TableFilterContext value)
        {
            BaseFilterDataSet(key, value, OperatorEnumeration.None);
        }

        private void BaseFilterDataSet(string key, TableFilterContext value, OperatorEnumeration operatorAction)
        {
            if (value.Value == null)
                return;

            switch (value.MatchMode)
            {
                case FilterTypeMatchModeStartsWith:
                    _linqOperator.AddFilterProperty(key.FirstCharToUpper(), value.Value,
                        LinqOperatorConstants.ConstantStartsWith
                        , operatorAction);
                    break;

                case FilterTypeMatchModeContains:
                    _linqOperator.AddFilterProperty(key.FirstCharToUpper(), value.Value,
                        LinqOperatorConstants.ConstantContains
                        , operatorAction);
                    break;

                case FilterTypeMatchModeIn:
                    _linqOperator.AddFilterListProperty(key.FirstCharToUpper(), value.Value
                        , operatorAction);
                    break;

                case FilterTypeMatchModeEndsWith:
                    _linqOperator.AddFilterProperty(key.FirstCharToUpper(), value.Value,
                        LinqOperatorConstants.ConstantEndsWith
                        , OperatorEnumeration.None);
                    break;

                case FilterTypeMatchModeEquals:
                    _linqOperator.AddFilterProperty(key.FirstCharToUpper(), value.Value,
                        LinqOperatorConstants.ConstantEquals
                        , operatorAction);
                    break;

                case FilterTypeMatchModeNotContains:
                    _linqOperator.AddFilterProperty(key.FirstCharToUpper(), value.Value,
                        LinqOperatorConstants.ConstantContains
                        , OperatorEnumeration.None, true);
                    break;

                case FilterTypeMatchModeNotEquals:
                    _linqOperator.AddFilterProperty(key.FirstCharToUpper(), value.Value,
                        LinqOperatorConstants.ConstantEquals
                        , operatorAction, true);
                    break;
                case FilterTypeMatchModeDateIs:
                    _linqOperator.AddFilterProperty(key.FirstCharToUpper(), value.Value,
                        LinqOperatorConstants.ConstantDateIs
                        , operatorAction);
                    break;
                case FilterTypeMatchModeDateIsNot:
                    _linqOperator.AddFilterProperty(key.FirstCharToUpper(), value.Value,
                        LinqOperatorConstants.ConstantDateIs
                        , operatorAction,true);
                    break;
                case FilterTypeMatchModeDateBefore:
                    _linqOperator.AddFilterProperty(key.FirstCharToUpper(), value.Value,
                        LinqOperatorConstants.ConstantBefore
                        , operatorAction);
                    break;
                case FilterTypeMatchModeDateAfter:
                    _linqOperator.AddFilterProperty(key.FirstCharToUpper(), value.Value,
                        LinqOperatorConstants.ConstantAfter
                        , operatorAction);
                    break;
                case FilterTypeMatchModeLessThan:
                    _linqOperator.AddFilterProperty(key.FirstCharToUpper(), value.Value,
                        LinqOperatorConstants.ConstantLessThan
                        , operatorAction);
                    break;
                case FilterTypeMatchModeLessOrEqualsThan:
                    _linqOperator.AddFilterProperty(key.FirstCharToUpper(), value.Value,
                        LinqOperatorConstants.ConstantLessThanOrEqual
                        , operatorAction);
                    break;
                case FilterTypeMatchModeGreaterThan:
                    _linqOperator.AddFilterProperty(key.FirstCharToUpper(), value.Value,
                        LinqOperatorConstants.ConstantGreaterThan
                        , operatorAction);
                    break;
                case FilterTypeMatchModeGreaterOrEqualsThan:
                    _linqOperator.AddFilterProperty(key.FirstCharToUpper(), value.Value,
                        LinqOperatorConstants.ConstantGreaterThanOrEqual
                        , operatorAction);
                    break;


                default:
                    throw new System.ArgumentException("Invalid Match mode!");
            }
        }

        public void FiltersDataSet(string key, IEnumerable<TableFilterContext> values)
        {
            foreach (var filterContext in values)
            {
                var operatorEnum = OperatorConstant.ConvertOperatorEnumeration(filterContext.Operator);
                BaseFilterDataSet(key, filterContext, operatorEnum);
            }
        }

        public void ExecuteFilter() => _linqOperator.WhereExecute();

        public IQueryable<TEntity> GetResult() => _linqOperator.GetResult();
    }
}
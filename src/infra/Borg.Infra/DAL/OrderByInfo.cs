using Borg.Infra.DDD;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Borg.Infra.DAL
{
    public class OrderByInfo<T> : OrderByInfo<T, dynamic> where T : class
    {
        public OrderByInfo(string directive) : base(directive)
        {
        }

        public OrderByInfo(Expression<Func<T, dynamic>> property, bool ascending = true) : base(property, ascending)
        {
        }
    }

    public class OrderByInfo<T, TKey> : ValueObject<OrderByInfo<T, TKey>> where T : class
    {
        private static readonly ConcurrentDictionary<Type, IEnumerable<OrderByInfo<T>>> DeclaredOrderbys =
            new ConcurrentDictionary<Type, IEnumerable<OrderByInfo<T>>>();

        public bool Ascending;

        public Expression<Func<T, TKey>> Property;

        public OrderByInfo(string directive)
        {
            SetDirective(directive);
            TruePropertyName = TruePropertyNameInternal(Property);
        }

        public OrderByInfo(Expression<Func<T, TKey>> property, bool ascending = true)
        {
            if (property == null) throw new ArgumentNullException(nameof(property));
            Property = property;
            Ascending = ascending;
            TruePropertyName = TruePropertyNameInternal(Property);
        }

        public string TruePropertyName { get; }

        public OrderByInfo<T> Dynamic => new OrderByInfo<T>(PropertyDynamicInternal(Property), Ascending);

        public string Directive => $"{TruePropertyName}:" + (Ascending ? "ASC" : "DESC");

        public static IEnumerable<OrderByInfo<T>> DefaultSorter()
        {
            var type = typeof(T);

            if (!DeclaredOrderbys.ContainsKey(type))
            {
                var hits = type.GetProperties(BindingFlags.Instance | BindingFlags.Public).Where(p =>
                    p.CustomAttributes.Any(c => c.AttributeType == typeof(OrderByAttribute)));
                if (hits.Any())
                {
                    var internalDict =
                    (from source in hits
                     let attr = source.GetCustomAttribute<OrderByAttribute>()
                     select Tuple.Create(source.Name, attr.Precedence, source, attr)).ToList();
                    var ords = new List<OrderByInfo<T>>();
                    foreach (var tuple in internalDict.OrderByDescending(x => x.Item2))
                    {
                        var parameterExpression = Expression.Parameter(typeof(T), type.Name.Substring(0, 1));
                        var memberExpression = Expression.Property(parameterExpression, tuple.Item3);
                        Expression conversion = Expression.Convert(memberExpression, typeof(object));
                        var composedLambdaExpression =
                            Expression.Lambda<Func<T, dynamic>>(conversion, tuple.Item1, new[] { parameterExpression });
                        ords.Add(new OrderByInfo<T>(composedLambdaExpression, tuple.Item4.Ascending));
                    }
                    DeclaredOrderbys[type] = ords;
                }
            }
            return DeclaredOrderbys[type];
        }

        public override string ToString()
        {
            return $"{typeof(T).Name}.{TruePropertyName}|" + (Ascending ? "ASC" : "DESC");
        }

        private void SetDirective(string directive)
        {
            if (string.IsNullOrWhiteSpace(directive.Trim())) throw new ArgumentNullException(nameof(directive));

            var input = directive.Trim().Split(new[] { ':' }, StringSplitOptions.RemoveEmptyEntries);
            var propName = input[0].Trim();
            var prop = GetProperty(typeof(T), propName);

            var parameterExpression = Expression.Parameter(typeof(T), typeof(T).Name.ToLower().Substring(0, 1));
            var memberExpression = Expression.Property(parameterExpression, prop);
            Expression
                conversion =
                    Expression.Convert(memberExpression,
                        typeof(TKey)); //TODO: Do I need the conversion only for the subclasses?
            //Expression<Func<T, TKey>> composedLambdaExpression = Expression.Lambda<Func<T, TKey>>(memberExpression, new ParameterExpression[] { parameterExpression });
            var composedLambdaExpression =
                Expression.Lambda<Func<T, TKey>>(conversion, propName, new[] { parameterExpression });
            Property = composedLambdaExpression;

            Ascending = !(input.Length == 2 && input[1].ToUpper() == "DESC");
        }

        private static Expression<Func<T, dynamic>> PropertyDynamicInternal(Expression<Func<T, TKey>> source)
        {
            Expression conversion = Expression.Convert(source, typeof(object));
            var composedLambdaExpression =
                Expression.Lambda<Func<T, dynamic>>(conversion, source.Parameters[0].Name, source.Parameters);
            return composedLambdaExpression;
        }

        private static string TruePropertyNameInternal(Expression<Func<T, TKey>> expression)
        {
            if (expression == null)
                throw new ArgumentNullException(nameof(expression));

            var memberExpr = expression.Body as MemberExpression;
            if (memberExpr == null)
            {
                var unaryExpr = expression.Body as UnaryExpression;
                if (unaryExpr != null && unaryExpr.NodeType == ExpressionType.Convert)
                    memberExpr = unaryExpr.Operand as MemberExpression;
            }

            if (memberExpr != null && memberExpr.Member.MemberType == MemberTypes.Property)
            {
                var prp = $"{expression.Parameters[0].Name}.";
                var nme = $"{memberExpr.Expression}.{memberExpr.Member.Name}";
                if (nme.StartsWith(prp)) nme = nme.Substring(prp.Length);
                return nme;
            }

            throw new ArgumentException("No property reference expression was found.", nameof(expression));
        }

        private static PropertyInfo GetProperty(Type type, string propName)
        {
            var prop = type.GetProperty(propName, BindingFlags.Public | BindingFlags.Instance);
            if (prop == null) throw new ArgumentException(nameof(prop));
            return prop;
        }
    }
}
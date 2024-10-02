В базе данных MS SQL Server есть продукты и категории. Одному продукту может соответствовать много категорий, в одной категории может быть много продуктов. Напишите SQL запрос для выбора всех пар «Имя продукта – Имя категории». Если у продукта нет категорий, то его имя все равно должно выводиться.

```sql
select 
  p.Name as ProductName,
  c.Name as CategoryName
from
  Products p
left join 
  ProductCategories pc on p.Id = pc.ProductId
left join 
  Categories c on pc.CategoryId = c.Id

using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Web.Mvc;
using System.Web.Mvc.Html;

namespace Loader.Helper
{
    public static class HtmlHelperExtension
    {


        public static MvcHtmlString EditorForTree<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> propertyExpression, Loader.ViewModel.TreeViewParam param) where TModel : class
        {
            var htmlBuilder = new StringBuilder();
            var htmlFieldName = ExpressionHelper.GetExpressionText(propertyExpression);
            var htmlFieldNameWithPrefix = html.ViewData.TemplateInfo.GetFullHtmlFieldName(htmlFieldName);
            var htmlFieldIdWithPrefix = html.ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldId(htmlFieldName);

            var value = ModelMetadata.FromLambdaExpression(propertyExpression, html.ViewData).Model;
            //   var finalvalue = value;
            //if(param.SelectedNodeId==0)
            //{
            //   finalvalue = null;
            //}
            var valueText = param.SelectedNodeText == null ? "" : param.SelectedNodeText;


            htmlBuilder.AppendFormat(@"<div class='input-group section-treeview' id=""{0}"">", htmlFieldIdWithPrefix);
            htmlBuilder.AppendFormat(@"<input type=""hidden"" name=""{0}"" class='internal-value' value=""{1}"" />", htmlFieldNameWithPrefix, param.SelectedNodeId);
            htmlBuilder.AppendFormat(@"<input type='text' name='display-txt' class='form-control display-txt' value=""{0}"" autocomplete='off' onkeydown ='return false'id=""{1}"" placeholder='Search...' style='max-width:1000px;'>", valueText, htmlFieldIdWithPrefix);
            htmlBuilder.AppendFormat(@"<span class='input-group-btn'>");
            htmlBuilder.AppendFormat(@"<button type = 'button' name='search' class='btn btn-flat btn-treeview-popup'
                         allowselectgroup=""{0}"" withimageicon=""{1}"" 
                        withcheckbox=""{2}"" excludeme=""{3}"" poptitle=""{4}"">", param.AllowSelectGroup, param.WithImageIcon, param.WithCheckBox, param.WithOutMe, param.Title);
            htmlBuilder.AppendFormat(@"<i class='fa fa-search'></i>");
            htmlBuilder.AppendFormat(@"</button>");
            htmlBuilder.AppendFormat(@"</span>");
            htmlBuilder.AppendFormat(@"</div>");

            return new MvcHtmlString(htmlBuilder.ToString());

        }
        public static MvcHtmlString EditorForEmployeeSearch<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> propertyExpression, Loader.ViewModel.SearchViewModel searchParam) where TModel : class
        {
            var htmlBuilder = new StringBuilder();
            var htmlFieldName = ExpressionHelper.GetExpressionText(propertyExpression);
            var htmlFieldNameWithPrefix = html.ViewData.TemplateInfo.GetFullHtmlFieldName(htmlFieldName);
            var htmlFieldIdWithPrefix = html.ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldId(htmlFieldName);

            var value = ModelMetadata.FromLambdaExpression(propertyExpression, html.ViewData).Model;
            var valueText = searchParam.Name == null ? "" : searchParam.Name;
            var address = searchParam.Address;


            htmlBuilder.AppendFormat(@"<div class='input-group section-search' id=""{0}"">", htmlFieldIdWithPrefix);
            htmlBuilder.AppendFormat(@"<input type=""hidden"" name=""{0}"" class='internal-value' value=""{1}"" />", htmlFieldNameWithPrefix, value);
            htmlBuilder.AppendFormat(@"<input type='text' name='display-txt' class='form-control display-txt employee-display' autocomplete='off' value=""{0}""onkeydown ='return false' id=""{1}"" placeholder='Search...' style='max-width:1000px;'>", "", htmlFieldIdWithPrefix);
            htmlBuilder.AppendFormat(@"<span class='input-group-btn'>");
            htmlBuilder.AppendFormat(@"<button type = 'button' name='search' class='btn btn-flat btn-search-popup'title=""{0}""
                         id=""{1}"" name=""{2}""phonenumber=""{3}"" address=""{4}""DeptName=""{5}""DesignationName=""{6}""searchFor=""{7}"">"
                        , searchParam.Title, value, searchParam.Name, searchParam.PhoneNumber, searchParam.Address,searchParam.DeptId,searchParam.DGId,searchParam.SearchFor);
            htmlBuilder.AppendFormat(@"<i class='fa fa-search'></i>");
            htmlBuilder.AppendFormat(@"</button>");
            htmlBuilder.AppendFormat(@"</span>");
            htmlBuilder.AppendFormat(@"</div>");

            return new MvcHtmlString(htmlBuilder.ToString());

        }


        public static MvcHtmlString EditorForList<TModel, TValue>(this HtmlHelper<TModel> html,
               Expression<Func<TModel, IEnumerable<TValue>>> propertyExpression,
               Expression<Func<TValue, string>> indexResolverExpression = null,
               string parentName = null,
               bool includeIndexField = true) where TModel : class
        {
            var items = propertyExpression.Compile()(html.ViewData.Model);
            var htmlBuilder = new StringBuilder();
            var htmlFieldName = ExpressionHelper.GetExpressionText(propertyExpression);
            if (parentName != null)
            {
                htmlFieldName = parentName + '.' + htmlFieldName;
            }
            var htmlFieldNameWithPrefix = html.ViewData.TemplateInfo.GetFullHtmlFieldName(htmlFieldName);

            Func<TValue, string> indexResolver = null;
            if (indexResolverExpression == null)
            {
                indexResolver = x => null;
            }
            else
            {
                indexResolver = indexResolverExpression.Compile();
            }
            foreach (var item in items)
            {
                var dummy = new { Item = item };
                var guid = indexResolver(item);
                var memberExp = Expression.MakeMemberAccess(Expression.Constant(dummy), dummy.GetType().GetProperty("Item"));
                var singleItemExp = Expression.Lambda<Func<TModel, TValue>>(memberExp, propertyExpression.Parameters);

                if (String.IsNullOrEmpty(guid))
                {
                    guid = Guid.NewGuid().ToString();
                }
                else
                {
                    guid = html.AttributeEncode(guid);
                }
                if (includeIndexField)
                {
                    htmlBuilder.Append(_EditorForListIndexField<TValue>(htmlFieldNameWithPrefix, guid, indexResolverExpression));
                }
                htmlBuilder.Append(html.EditorFor(singleItemExp, null, string.Format("{0}[{1}]", htmlFieldName, guid)));
            }
            return new MvcHtmlString(htmlBuilder.ToString());
        }

        private static MvcHtmlString EditorForListIndexField<TModel>(this HtmlHelper<TModel> html, Expression<Func<TModel, string>> indexResolverExpression = null)
        {
            var htmlPrefix = html.ViewData.TemplateInfo.HtmlFieldPrefix;
            var first = htmlPrefix.LastIndexOf('[');
            var last = htmlPrefix.IndexOf(']', first + 1);
            if (first == -1 || last == -1)
            {
                throw new InvalidOperationException("EditorForListIndexField called when not in a EditorForList context");

            }
            var htmlFieldNameWithPrefix = htmlPrefix.Substring(0, first);
            var guid = htmlPrefix.Substring(first + 1, last - first - 1);

            return _EditorForListIndexField<TModel>(htmlFieldNameWithPrefix, guid, indexResolverExpression);

        }

        private static MvcHtmlString _EditorForListIndexField<TModel>(string htmlFieldNameWithPrefix, string guid, Expression<Func<TModel, string>> indexResolverExpression)
        {
            var htmlBuilder = new StringBuilder();
            htmlBuilder.AppendFormat(@"<input type=""hidden"" name=""{0}.Index"" value=""{1}"" />", htmlFieldNameWithPrefix, guid);

            if (indexResolverExpression != null)
            {
                htmlBuilder.AppendFormat(@"<input type=""hidden"" id=""ColIndex"" name=""{0}[{1}].{2}"" value=""{1}"" />", htmlFieldNameWithPrefix, guid, ExpressionHelper.GetExpressionText(indexResolverExpression));
            }

            return new MvcHtmlString(htmlBuilder.ToString());
        }
    }
}
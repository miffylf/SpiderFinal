<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Search.aspx.cs" Inherits="LuceneSearch.Search" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="css/ui-lightness/jquery-ui-1.8.2.custom.css" rel="stylesheet" type="text/css" />
    <script src="js/jquery-1.4.2.min.js" type="text/javascript"></script>
    <script src="js/jquery-ui-1.8.2.custom.min.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(function () {
            $("#txtKeyword").autocomplete(
            {   source: "SearchSuggestion.ashx",
                select: function (event, ui) { $("#txtKeywo‘rd").val(ui.item.value); $("#form1").submit(); }
            });
        });
    </script>
     <style type="text/css">
        .pager {
	        TEXT-ALIGN: center; PADDING-BOTTOM: 3px; PADDING-LEFT: 0px; PADDING-RIGHT: 0px; FLOAT: right; PADDING-TOP: 3px
        }
        .pager A {
	        BORDER-BOTTOM: #ccc 1px solid; TEXT-ALIGN: left; BORDER-LEFT: #ccc 1px solid; PADDING-BOTTOM: 3px; LINE-HEIGHT: 26px; MARGIN: 0px 2px; OUTLINE-STYLE: none; PADDING-LEFT: 5px; PADDING-RIGHT: 5px; BACKGROUND: #fff; COLOR: #000; FONT-SIZE: 12px; BORDER-TOP: #ccc 1px solid; BORDER-RIGHT: #ccc 1px solid; TEXT-DECORATION: none; PADDING-TOP: 4px
        }
        .pager A:hover {
	        BORDER-BOTTOM: #f80 1px solid; BORDER-LEFT: #f80 1px solid; COLOR: #f80; BORDER-TOP: #f80 1px solid; BORDER-RIGHT: #f80 1px solid; TEXT-DECORATION: underline
        }
        .pager A:focus {
                        
	        -moz-outline-style: none
        }
        .pager SPAN {
	        BORDER-BOTTOM-STYLE: none; TEXT-ALIGN: left; PADDING-BOTTOM: 4px; LINE-HEIGHT: 26px; BORDER-RIGHT-STYLE: none; MARGIN: 1px 2px; PADDING-LEFT: 6px; PADDING-RIGHT: 6px; BORDER-TOP-STYLE: none; BACKGROUND: #f80; COLOR: #fff; FONT-SIZE: 12px; BORDER-LEFT-STYLE: none; PADDING-TOP: 5px
        }
    </style>
        <style type="text/css">
        #hotwordsUL li{float:left;margin-left:150px;list-style-type:none;}
            #txtKeyword
            {
                width: 354px;
                margin-left: 0px;
            }
    </style>
</head>
<body>
    <form id="form1">
    <div align="center">
        <img src="http://incubator.apache.org/lucene.net/images/lucene-medium.png"/>
    </div>
    <br />
    <br />
    <div align="center">
        <input type="text" id="txtKeyword" name="kw" value='<%=kw %>'/>
        <%-- <asp:Button ID="createIndexButton" runat="server" onclick="searchButton_Click" 
            Text="创建索引库" />--%>
        <input type="submit" name="searchButton" value="搜索" style="width: 91px" /><br />
    </div>
    <br />
    <ul id="hotwordsUL">
          <asp:Repeater ID="hotwordsRepeater" runat="server">
            <ItemTemplate>
                <li><a href='CreateIndex.aspx?kw=<%#Eval("Keyword") %>'><%#Eval("Keyword") %></a></li>
            </ItemTemplate>
          </asp:Repeater>
    </ul>
    &nbsp;<br />
  
    <asp:Repeater ID="dataRepeater" runat="server" EnableViewState="true">
        <HeaderTemplate>
            <ul>
        </HeaderTemplate>
        <ItemTemplate>
            <li>
                <a href='http://localhost:8080/showtopic-<%#Eval("Number") %>.aspx'><%#Eval("Title") %></a>
                <br />
                <%#Eval("BodyPreview") %>
            
            </li>
        </ItemTemplate>
        <FooterTemplate>
        </ul>
        </FooterTemplate>
    </asp:Repeater>
    <br />
    <div class="pager"><%=RenderToHTML%></div>

    </form>
</body>
</html>

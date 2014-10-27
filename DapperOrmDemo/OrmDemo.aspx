<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="OrmDemo.aspx.cs" Inherits="DbAccessDemo.OrmDemo" %>
<%@ Register assembly="GuoXin.WebControls" namespace="GuoXin.WebControls" tagprefix="cc1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title></title>
    <style type="text/css">
        #form1
        {
           
            width: 1024;
            margin: Auto;
        }
        .style1
        {
            width: 50px;
        }
        .style2
        {
            width: 160px;
        }
        .style3
        { font-size:15px;
            }
        .styleHeigh
        {
            height:22px;
        }
         .styleWidth
        {
           width:453px;
        }
        .styleWidth1
        {
            width:50px;
            }
               
    </style>
    <script language="javascript" type="text/javascript">
        // <![CDATA[

        function btnSave_onclick() {

        }

        // ]]>
    </script>
</head>
<body>
    <form id="form1" runat="server"  >
     <div style="height:40px; display:table-header-group" >
        <label>ORM演示Demo</label>
    </div>
    
      <table border="2PX" >
        <tr>
          <td class="style1"><label>EmployyedId: </label> </td>
          <td ><asp:TextBox ID="txbEmployyedId" runat="server" 
                  ontextchanged="txbEmployyedId_TextChanged"></asp:TextBox></td>
        </tr>
        <tr>
          <td><label>DepatrmentId:&nbsp;&nbsp; </label> 
          </td>
          <td ><asp:TextBox ID="txbDempartmentId" runat="server"></asp:TextBox>
          </td>
        </tr>
        <tr>
          <td><label>FullName:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; </label>
          </td>
          <td ><asp:TextBox ID="txbFullName" runat="server"></asp:TextBox>
          </td>
        </tr>
        <tr>
          <td><label>Gender:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; </label> 
          </td>
          <td ><asp:DropDownList ID="DropDownList1" runat="server">
             </asp:DropDownList> 时间格式2010-10-10
          </td>
        </tr>
        <tr>
          <td><label>Birthday: </label>
          </td>
          <td ><asp:TextBox 
                 ID="txbBirthdayStart" runat="server"></asp:TextBox>
             到<asp:TextBox ID="txbBirthdayEnd" runat="server"></asp:TextBox>
          </td>
        </tr>
        <tr>
          <td><label>CreateDate:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; </label>
          </td>
          <td ><asp:TextBox ID="txbCreateDateStart" runat="server"></asp:TextBox>到<asp:TextBox 
                 ID="txbCreateDateEnd" runat="server"></asp:TextBox>
          </td>
        </tr>
        <tr>
          <td></td>
          <td align="right"  ><asp:Button ID="btnSearch" runat="server" onclick="btnSearch_Click" 
        Text="Search" style="height: 26px" /></td>
        </tr>
        <tr><td></td><td align=right  >
            <asp:Button ID="btnClear" runat="server" Text="Clear" 
                onclick="btnClear_Click" />
            </td></tr>
      </table> 
      <div class="styleHeigh"></div>
     <div >        
         <label>KeySearch: </label>
         <asp:TextBox ID="txbKeySearch" runat="server"></asp:TextBox>
         &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;   &nbsp;  
          <asp:Button ID="Button1" 
    runat="server" Text="KeySearch" Height="24px" onclick="Button1_Click1" /> 
</div>
 <div class="styleHeigh" ></div>
   <cc1:GXGridView ID="gvDemo" runat="server">
    </cc1:GXGridView>
     <div class="styleHeigh" ></div>
    <div><asp:Label ID="lblPages" runat="server" CssClass="style3"></asp:Label>
     <asp:TextBox ID="txtPageSize" CssClass="style3" runat="server"
            style="height: 18px" Width="22px" 
              Height="18px">5</asp:TextBox>  
         <asp:Button ID="btnSetNum" runat="server" Text="SetSize" 
            onclick="btnSetNum_Click" /> <label class="style3">
        转到</label><asp:TextBox ID="txtPage" CssClass="style3" runat="server"
         style="height: 18px" Width="22px"></asp:TextBox>
    <label class="style3">页</label><asp:Button ID="btnSetIndex" runat="server" 
            style="height: 22px" Text="SetIndex" onclick="btnSetIndex_Click" />
    </div>
    
   <br />
    <table style="width:100%;">
        <tr>
            <td class="style1">
                输入需要修改员工号</td>
            <td class="style2">
                <asp:TextBox ID="txtSearchId"  runat="server" ></asp:TextBox>
            </td>
            <td>
                <asp:Button ID="btnSearch1" runat="server" CssClass="styleWidth1" 
                    onclick="btnSearch1_Click" Text="查找" Enabled="True" />
                
                </td>
        </tr>
        <tr>
            <td class="style1">
               EmployeeId:</td>
            <td class="style2">
                <asp:TextBox runat="server" ID="txtId"></asp:TextBox>
            </td>
            <td>&nbsp;</td>
        </tr>
        <tr>
            <td class="style1">
                DepartmentId:</td>
            <td class="style2">
                <asp:TextBox ID="txtDepId"  runat="server"></asp:TextBox>
            </td>
            <td>
                <asp:Button ID="btnDel" runat="server" CssClass="styleWidth1" 
                    onclick="btnDel_Click" Text="Delete" Enabled="False" />
                
                <asp:Button ID="btnUpdate" runat="server" Text="Update" CssClass="styleWidth1" 
                    onclick="btnUpdate_Click" Enabled="False" /> 
                <asp:Button ID="btnCance" runat="server" onclick="Button2_Click" Text="Cance" 
                    Enabled="False" />
             </td>
        </tr>
        <tr>
            <td class="style1">
                FullName:</td>
            <td class="style2">
                <asp:TextBox ID="txtFullName" runat="server" style=" margin:Auto "></asp:TextBox>
            </td>
            <td>
               </td>
        </tr>
                <tr>
            <td class="style1">
               Gender:</td>
            <td class="style2">
                <asp:DropDownList ID="DddlistGender" runat="server">
                </asp:DropDownList>
                    </td>
            <td>
                &nbsp;</td>
        </tr>
        <tr>
            <td class="style1">
                Birthday</td>
            <td class="style2">
               <asp:TextBox ID="txtBirthday" runat="server"></asp:TextBox></td>
            <td>
                               <asp:Button ID="btnAdd" runat="server" CssClass="styleWidth1" Text="Add" 
                                   Width="50px" onclick="btnAdd_Click"/> 
                <asp:Button ID="btnSave" runat="server" CssClass="styleWidth1"  Text="Save" 
                                   onclick="btnSave_Click1" Enabled="False"/>
                                   
</td>
        </tr>
      
                  <tr>
            <td class="style1">
                CreateDate</td>
            <td class="style2">
               <asp:TextBox ID="txtCreateDate" runat="server"></asp:TextBox></td>
            <td>
                             
                                   
</td>
        </tr>


    </table>
   
      
   

    </form>
</body>
</html>

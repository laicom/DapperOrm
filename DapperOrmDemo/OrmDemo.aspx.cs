using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DapperOrm.Model;
using DapperOrm;
using GuoXin.WebControls;
using DbAccessDemo.Model;


namespace DbAccessDemo
{
    public partial class OrmDemo : System.Web.UI.Page
    {
        protected IOrmDal orm = OrmDal.Create("SqlServerDb");
        protected EmployeeInfo employee;

        protected void Page_Load(object sender, EventArgs e)
        {
            gvDemo.OnGetData += new GetDataDelegate(orm.GetList<EmployeeInfo>);
            if (!IsPostBack)
            {
                //增加一条排序规则
                gvDemo.Criteria.AddOrderRule("EmployeeID", Order.ASC);
                gvDemo.Criteria.PageSize = 5;
                //gvDemo.Criteria.AddCriteria("Test", null, MatchType.StringEqual | MatchType.Not);
                //页大小
                gvDemo.PageSize = 5;
              
                #region 性别
                ListItem item = new ListItem("性别", "");
                ListItem item0 = new ListItem("男", "男");
                ListItem item1 = new ListItem("女", "女");

                DddlistGender.Items.Add(item);
                DddlistGender.Items.Add(item0);
                DddlistGender.Items.Add(item1);

                DropDownList1.Items.Add(item);
                DropDownList1.Items.Add(item0);
                DropDownList1.Items.Add(item1);
                #endregion
              
                RefreshList();

            }
        }

        private void RefreshList()
        {
            gvDemo.Criteria.PageCount = 0;
            //默认显示
            gvDemo.ShowList();
            //分页状态显示
            lblPages.Text = string.Format("总记录数：{0},每页显示{1}条，当前{2}/{3}页，设置页大小：", gvDemo.RecordCount, gvDemo.PageSize, gvDemo.PageIndex + 1, gvDemo.PageCount);
        }

        protected void btnSetNum_Click(object sender, EventArgs e)
        {//设置单页数据量

            try
            {
                int size = 5;
                if (string.IsNullOrEmpty(txtPageSize.Text.Trim()))
                {
                    gvDemo.PageIndex = 0;
                    txtPage.Text = string.Empty;
                    gvDemo.PageSize = 0;
                    RefreshList();
                }

                else if (int.TryParse(txtPageSize.Text, out size))
                {
                    gvDemo.PageIndex = 0;
                    txtPage.Text = string.Empty;
                    gvDemo.PageSize = size;
                    RefreshList();
                }


            }
            catch
            { 

            }
        }
        protected void btnSetIndex_Click(object sender, EventArgs e)
        {//进入页数

            try
            {
                int index = 1;
                if (index >= 0 && index < gvDemo.Criteria.PageCount)
                {
                    if (string.IsNullOrEmpty(txtPage.Text))
                    {
                        gvDemo.PageIndex = 0;
                        RefreshList();
                    }
                    else if (int.TryParse(txtPage.Text, out index))
                    {
                        gvDemo.PageIndex = --index;
                        RefreshList();
                    }

                }

            }

            catch
            { }
          
        }


        protected void Button1_Click(object sender, EventArgs e)
        {
            try
            {
                gvDemo.Criteria.AddCriteria("DepartmentId", txtDepId.Text, MatchType.In, "");

                gvDemo.Criteria.Criterias.Clear();
            }
            catch
            { }

        }

        protected void btnDel_Click(object sender, EventArgs e)
        {
            try
            {
                int roleId = 0;
                Int32.TryParse(txtId.Text, out roleId);
                if (roleId != 0)
                    orm.RemoveById<EmployeeInfo>(roleId);
                RefreshList();
                btnDel.Enabled = false;
                btnUpdate.Enabled = false;
                btnCance.Enabled = false;
            }
            catch
            { }
        }

        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                employee = new EmployeeInfo();
                employee.EmployeeId = int.Parse(txtId.Text.Trim());
                employee.DepartmentId = int.Parse(txtDepId.Text.Trim());
                employee.FullName = txtFullName.Text.Trim();
                employee.Sex = DddlistGender.SelectedValue;// bool.Parse(DddlistGender.SelectedValue);
                employee.Birthday = DateTime.Parse(txtBirthday.Text.Trim());
                employee.CreateDate = DateTime.Parse(txtCreateDate.Text.Trim());
                orm.Save<EmployeeInfo>(employee);


                RefreshList();
                btnDel.Enabled = false;
                btnUpdate.Enabled = false;
                btnCance.Enabled = false;

                txtId.Text = txtDepId.Text = txtFullName.Text = txtBirthday.Text = txtCreateDate.Text = string.Empty;
                DddlistGender.SelectedValue = DddlistGender.Items[0].Value;
            }
            catch
            { }
        }
        protected void Button1_Click1(object sender, EventArgs e)
        {
            try
            {
                gvDemo.Criteria.Key = txbKeySearch.Text.Trim();
                RefreshList();
                txbKeySearch.Text = string.Empty;

            }
            catch
            { }
        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                btnDel.Enabled = false;
                btnUpdate.Enabled = false;
                btnCance.Enabled = false;

                txtId.Text = txtDepId.Text = txtFullName.Text = txtBirthday.Text = txtCreateDate.Text = string.Empty;
                DddlistGender.SelectedValue = DddlistGender.Items[0].Value;
                btnSave.Enabled = true;
            }
            catch
            { }
        }

        protected void btnSave_Click1(object sender, EventArgs e)
        {
            EmployeeInfo employeeInfo;
            try
            {
                employeeInfo = new EmployeeInfo();
                employeeInfo.DepartmentId = int.Parse(txtDepId.Text.Trim());
                employeeInfo.FullName = txtFullName.Text.Trim();
                employeeInfo.Sex = DddlistGender.SelectedValue;// bool.Parse(DddlistGender.SelectedValue);
                employeeInfo.Birthday = DateTime.Parse(txtBirthday.Text.Trim());
                employeeInfo.CreateDate = DateTime.Parse(txtCreateDate.Text.Trim());
                orm.Save(employeeInfo);
                txtId.Text = employeeInfo.EmployeeId.ToString();
                RefreshList();

                btnSave.Enabled = false;
                txtId.Text = txtDepId.Text = txtFullName.Text = txtBirthday.Text = txtCreateDate.Text = string.Empty;
            }
            catch
            {

            }
        }


        protected void btnSearch1_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtSearchId.Text.Trim()))
                return;
            try
            {
                //gvDemo.Criteria.AddCriteria("EmployeeId", int.Parse(txtSearchId.Text.Trim()), MatchType.Equal, "");
                //RefreshList();
                //gvDemo.Criteria.Criterias.Clear();
                int employeeId= int.Parse(txtSearchId.Text.Trim());
            
                    btnSave.Enabled = false;
                    btnDel.Enabled = true;
                    btnUpdate.Enabled = true;
                    btnCance.Enabled = true;


                    employee = orm.GetObjectById<EmployeeInfo>(employeeId); // new EmployeeInfo();
                    txtId.Text = employee.EmployeeId.ToString();
                    txtDepId.Text = employee.DepartmentId.ToString();
                    txtFullName.Text = employee.FullName;
                    employee.Sex = DddlistGender.SelectedValue;
                   
                    //if (employee.Sex)
                    //{
                    //    DddlistGender.SelectedValue = DddlistGender.Items[1].Value;
                    //}
                    //else
                    //{
                    //    DddlistGender.SelectedValue = DddlistGender.Items[2].Value;
                    //}

                    txtBirthday.Text = employee.Birthday.ToString("yyyy-MM-dd");
                    txtCreateDate.Text = employee.CreateDate.ToString("yyyy-MM-dd");
               
            }
            catch
            { }
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {

            try
            {//联合查询

                gvDemo.Criteria.Criterias.Clear();
                gvDemo.Criteria.AddCriteria("EmployeeId", txbEmployyedId.Text.Trim(), MatchType.In, "");
                gvDemo.Criteria.AddCriteria("DepartmentId", txbDempartmentId.Text.Trim(), MatchType.In, "");
                gvDemo.Criteria.AddCriteria("FullName", txbFullName.Text.Trim(), MatchType.WildCard, "");
                //if (DropDownList1.SelectedValue != "性别")
                //{
                    gvDemo.Criteria.AddCriteria("Gender", DropDownList1.SelectedValue , MatchType.StringEqual, "");

                //} 
                if ( !string.IsNullOrEmpty(txbBirthdayStart.Text.Trim()))
                gvDemo.Criteria.AddCriteria("Birthday", DateTime.Parse(txbBirthdayStart.Text.Trim()), MatchType.TimeFrom);
                if (!string.IsNullOrEmpty(txbBirthdayEnd.Text.Trim()))
                gvDemo.Criteria.AddCriteria("Birthday", DateTime.Parse(txbBirthdayEnd.Text.Trim()), MatchType.TimeTo);
                if (!string.IsNullOrEmpty(txbCreateDateStart.Text.Trim()))
                gvDemo.Criteria.AddCriteria("CreateDate", DateTime.Parse(txbCreateDateStart.Text.Trim()), MatchType.TimeFrom, "");
                if (!string.IsNullOrEmpty(txbCreateDateEnd.Text.Trim()))
                gvDemo.Criteria.AddCriteria("CreateDate", DateTime.Parse(txbCreateDateEnd.Text.Trim()), MatchType.TimeTo, "");
                gvDemo.Criteria.PageIndex = 0;
                RefreshList();
               

            }
            catch
            {

                gvDemo.Criteria.Criterias.Clear();
                RefreshList();
            }


        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            txbEmployyedId.Text=txbDempartmentId.Text=txbFullName.Text=txbBirthdayStart.Text=txbBirthdayEnd.Text=txbCreateDateEnd.Text
                =txbCreateDateStart.Text=string.Empty;
            DropDownList1.SelectedValue = DropDownList1.Items[0].Value;
                   
        }

        protected void Button2_Click(object sender, EventArgs e)
        {
            btnCance.Enabled = false;
            btnUpdate.Enabled = false;
            btnDel.Enabled = false;
            RefreshList();

            txtId.Text = txtDepId.Text = txtFullName.Text = txtBirthday.Text = txtCreateDate.Text = string.Empty;
            DddlistGender.SelectedValue = DddlistGender.Items[0].Value;

        }

        protected void txbEmployyedId_TextChanged(object sender, EventArgs e)
        {

        }
    }
}

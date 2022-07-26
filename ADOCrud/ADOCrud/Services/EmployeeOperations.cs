﻿using ADOCrud.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;

namespace ADOCrud.Services
{
    public class EmployeeOperations : IEmployeeOperations
    {
        SqlConnection cn;
        public EmployeeOperations()
        {
            cn = new SqlConnection(@"Data Source=DESKTOP-52IAUQR;Initial Catalog=Company;Integrated Security=True");
        }

        public List<tblemployee> GetEmployees()
        {
            SqlDataAdapter da = new SqlDataAdapter("exec GetAllEmployees 0,0", cn);
            DataTable dt = new DataTable();
            da.Fill(dt);

            List<tblemployee> li = new List<tblemployee>();
            foreach (DataRow item in dt.Rows)
            {
                li.Add(new tblemployee
                {
                    emp_id = Convert.ToInt32(item["emp_id"].ToString()),
                    f_name = item["f_name"].ToString(),
                    salary = Convert.ToInt32(item["salary"].ToString()),
                    email = item["email"].ToString(),
                    mobile = item["mobile"].ToString(),
                    gender = item["gender"].ToString(),
                    address = item["address"].ToString()
                });
            }
            return li;
        }

        public tblemployee GetEmployeeById(int id)
        {
            SqlDataAdapter da = new SqlDataAdapter("exec GetAllEmployees " + id + ",1", cn);
            DataTable dt = new DataTable();
            da.Fill(dt);

            tblemployee li = new tblemployee();
            if (dt.Rows.Count == 1)
            {
                li.emp_id = Convert.ToInt32(dt.Rows[0]["emp_id"].ToString());
                li.f_name = dt.Rows[0]["f_name"].ToString();
                li.salary = Convert.ToInt32(dt.Rows[0]["salary"].ToString());
                li.email = dt.Rows[0]["email"].ToString();
                li.mobile = dt.Rows[0]["mobile"].ToString();
                li.gender = dt.Rows[0]["gender"].ToString();
                li.address = dt.Rows[0]["address"].ToString();
            }
            return li;
        }

        public void InsertEmployee(tblemployee obj)
        {
            SqlCommand cmd = new SqlCommand("insert into tblemployee (f_name,salary,email,mobile,gender,address) values(@fn,@sl,@em,@mb,@gn,@ad)", cn);
            cmd.Parameters.AddWithValue("@fn", obj.f_name);
            cmd.Parameters.AddWithValue("@sl", obj.salary);
            cmd.Parameters.AddWithValue("@em", obj.email);
            cmd.Parameters.AddWithValue("@mb", obj.mobile);
            cmd.Parameters.AddWithValue("@gn", obj.gender);
            cmd.Parameters.AddWithValue("@ad", obj.address);

            cn.Open();
            cmd.ExecuteNonQuery();
            cn.Close();
        }
        public void UpdateEmployee(tblemployee obj)
        {
            SqlCommand cmd = new SqlCommand("update tblemployee set f_name=@fn,salary=@sl,email=@em,mobile=@mb,gender=@gn,address=@ad where emp_id=@id", cn);
            cmd.Parameters.AddWithValue("@fn", obj.f_name);
            cmd.Parameters.AddWithValue("@sl", obj.salary);
            cmd.Parameters.AddWithValue("@id", obj.emp_id);
            cmd.Parameters.AddWithValue("@em", obj.email);
            cmd.Parameters.AddWithValue("@mb", obj.mobile);
            cmd.Parameters.AddWithValue("@gn", obj.gender);
            cmd.Parameters.AddWithValue("@ad", obj.address);

            cn.Open();
            cmd.ExecuteNonQuery();
            cn.Close();
        }
        public void DeleteEmployee(int id)
        {
            SqlCommand cmd = new SqlCommand("delete from tblemployee where emp_id=@id", cn);
            cmd.Parameters.AddWithValue("@id", id);

            cn.Open();
            cmd.ExecuteNonQuery();
            cn.Close();
        }

    }
}
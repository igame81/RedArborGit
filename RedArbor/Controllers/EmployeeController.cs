using AutoMapper;
using log4net;
using RedArbor.Dao;
using RedArbor.Dao.Interface;
using RedArbor.DTO;
using RedArbor.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace RedArbor.Controllers
{
    public class EmployeeController : ApiController
    {

        private IEmployeeDao employeeDao;
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);


        public EmployeeController(IEmployeeDao employeeDao)
        {
            this.employeeDao = employeeDao;
        }

        // GET: api/redarbor
        public HttpResponseMessage Get()
        {
            try
            {
                EmployeeBaseDTO[] result = Mapper.Map<Employee[], EmployeeBaseDTO[]>(this.employeeDao.FindAll());
                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, result);
                Log.DebugFormat("Get employees: {0}", result.Count());
                return response;
            }
            catch(Exception ex)
            {
                Log.ErrorFormat("Error on FindAll {0}:", ex.ToString());
                return new HttpResponseMessage(HttpStatusCode.BadRequest);
            }
        }

        // GET: api/redarbor/5
        public HttpResponseMessage Get(int id)
        {
            try
            {
                EmployeeBaseDTO result = Mapper.Map<Employee, EmployeeBaseDTO>(this.employeeDao.FindById(id));
                if (result != null)
                {
                    HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, result);
                    return response;
                }
                else
                {
                    Log.DebugFormat("Employee with id {0} not found", id);
                    return new HttpResponseMessage(HttpStatusCode.NotFound);
                }
            }
            catch(Exception ex)
            {
                Log.ErrorFormat("Error on FindById {0}:", ex.ToString());
                return new HttpResponseMessage(HttpStatusCode.BadRequest);
            }
        }

        // POST: api/redarbor
        public HttpResponseMessage Post([FromBody]EmployeeBaseDTO employeeDTO)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    EmployeeBaseDTO result = Mapper.Map<Employee, EmployeeBaseDTO>(this.employeeDao.SaveOrUpdate(Mapper.Map<EmployeeBaseDTO, Employee>(employeeDTO)));
                    HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, result);
                    return response;
                }
                else
                {
                    HttpResponseMessage response = Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
                    return response;
                }
                
            }
            catch(Exception ex)
            {
                Log.ErrorFormat("Error post {0}:", ex.ToString());
                return new HttpResponseMessage(HttpStatusCode.BadRequest);
            }
        }

        // PUT: api/redarbor/5
        public HttpResponseMessage Put(int id, [FromBody]EmployeeBaseDTO employeeDTO)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (id != employeeDTO.Id)
                    {
                        Log.Debug("Id != employeeId on put");
                        return new HttpResponseMessage(HttpStatusCode.BadRequest);
                    }
                    else
                    {
                        EmployeeBaseDTO employee = Mapper.Map<Employee, EmployeeBaseDTO>(this.employeeDao.FindById(id));
                        if (employee != null)
                        {
                            EmployeeBaseDTO result = Mapper.Map<Employee, EmployeeBaseDTO>(this.employeeDao.SaveOrUpdate(Mapper.Map<EmployeeBaseDTO, Employee>(employeeDTO)));
                            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK);
                            return response;
                        }
                        else
                        {
                            Log.DebugFormat("Employee not found on Put, employeeId {0}", id);
                            return new HttpResponseMessage(HttpStatusCode.NotFound);
                        }
                        
                    }
                    
                }
                else
                {
                    Log.Debug("Model not valid in Put");
                    HttpResponseMessage response = Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
                    return response;
                }

            }
            catch(Exception ex)
            {
                Log.ErrorFormat("Error put {0}:", ex.ToString());
                return new HttpResponseMessage(HttpStatusCode.BadRequest);
            }
        }

        // DELETE: api/redarbor/5
        public HttpResponseMessage Delete(int id)
        {
            try
            {
                EmployeeBaseDTO employeeDTO = Mapper.Map<Employee, EmployeeBaseDTO>(this.employeeDao.FindById(id));
                if (employeeDTO == null)
                {
                    Log.DebugFormat("Employee not found on delete, employeeId {0}", id);
                    return new HttpResponseMessage(HttpStatusCode.NotFound);
                }
                else
                {
                    bool deleteOk = this.employeeDao.Delete(id);
                    if (!deleteOk)
                    {
                        return new HttpResponseMessage(HttpStatusCode.NotFound);
                    }
                }

                return new HttpResponseMessage(HttpStatusCode.OK);
               
            }
            catch(Exception ex)
            {
                Log.ErrorFormat("Error delete employee id {0}, error {1}:", id, ex.ToString());
                return new HttpResponseMessage(HttpStatusCode.BadRequest);
            }
        }
    }
}

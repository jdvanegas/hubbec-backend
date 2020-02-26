using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Domain.Common.OperationHandling
{
  public class OperationResult
  {
    public OperationResult()
    {
      Errors = new List<string>();
    }

    public bool Status => !Errors.Any();

    public List<string> Errors { get; set; }
  }
  public class OperationResult<T> : OperationResult
  {
    public T Data { get; set; }
  }
}
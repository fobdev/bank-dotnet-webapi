using System;
using Bank.Api.Dtos;
using Bank.Api.Models;
using Bank.Api.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Bank.Api.Controllers
{
    [ApiController]
    [Route("bank")]
    public class BankController : ControllerBase
    {
        private readonly IBankRepository _bank_repository;
        public BankController(IBankRepository bank_repository)
        {
            this._bank_repository = bank_repository;
        }

        // [GET] endpoint: users/{id}
        [HttpGet("{id:guid}")]
        public ActionResult<PhysicalBankDto> GetBankById(Guid id)
        {
            var bank = _bank_repository.GetBankById(id);
            if (bank is null) return NotFound();

            return bank.AsBankDto();
        }

        [HttpPost("create")]
        public ActionResult<PhysicalBankCreateDto> CreateBank(PhysicalBankCreateDto bankDto)
        {
            PhysicalBank bank = new()
            {
                id = Guid.NewGuid(),
                balance = bankDto.balance,
            };

            _bank_repository.CreateBank(bank);

            return CreatedAtAction(nameof(GetBankById), new { id = bank.id }, bank.AsBankDto());
        }

        [HttpPost("loan")]
        public ActionResult MakeLoan(LoanCreateDto loanDto)
        {

            // TODO

            return null;
        }
    }
}
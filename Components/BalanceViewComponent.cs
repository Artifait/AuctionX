using System;
using AucX.Domain.Entities;
using AucX.WebUI.Infrastructure;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AucX.WebUI.Components;

public class BalanceViewComponent : ViewComponent
{
    private readonly UserManager<AppUser> _userManager;
    private readonly IBalanceService _balanceService;

    public BalanceViewComponent(
        UserManager<AppUser> userManager,
        IBalanceService balanceService)
    {
        _userManager = userManager;
        _balanceService = balanceService;
    }

    public async Task<IViewComponentResult> InvokeAsync()
    {
        var user = await _userManager.GetUserAsync(HttpContext.User);
        if (user == null) return Content(string.Empty);

        var balance = await _balanceService.GetBalanceAsync(user.Id);
        var frozen = await _balanceService.GetFrozenBalanceAsync(user.Id);

        return View(new BalanceViewModel
        {
            Balance = balance,
            FrozenBalance = frozen
        });
    }
}

public class BalanceViewModel
{
    public decimal Balance { get; set; }
    public decimal FrozenBalance { get; set; }
}

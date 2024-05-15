#load ".bin/PxlLocalDev.fsx"
open PxlLocalDev

open Pxl
open Pxl.Ui

let w,h = 24,24

let canvas = createTestCanvas w h Target.Local Connection.Http
(*
let canvas = createTestCanvas w h Target.RPi Connection.SignalR
*)



// -------------------------------------------------------------


(*
    We want an "Old Movie Countdown" animation.
    For that, we need:
        1. - a background
        2. - a static thin centered circle
        3. - a number (as text) in the center that decrements every second
        4. - an animated arc, starting from 0 to 360 degrees, that completes in a second.
*)


Eval.off canvas


vide {
    bg(Colors.lightGray.opacity(0.5))
}
|> Eval.plot canvas



// -------------------------------------------------------------



// Background, circle and (static) text!
// We can access the context (ctx) to get
// the width and height of the canvas
// (... and more if we want to)
vide {
    bg(Colors.lightGray)

    let! ctx = getCtx ()

    circle(
        ctx.halfWidth, ctx.halfHeight, 12,
        stroke(Colors.black, 1.0))
}
|> Eval.plot canvas



// -------------------------------------------------------------



// Add the arc (that we will animated in the upcoming step).
vide {
    bg(Colors.lightGray)

    let! ctx = getCtx ()

    circle(
        ctx.halfWidth, ctx.halfHeight, 12,
        stroke(Colors.black, 1.0))

    let angle = 150.0
    arc(
        -ctx.width, -ctx.height,
        3 * ctx.width, 3 * ctx.height,
        -90,
        angle,
        fill(Colors.darkRed.opacity(0.8)))
}
|> Eval.startFixedFps 50 canvas



// -------------------------------------------------------------



vide {
    bg(Colors.lightGray)

    let! ctx = getCtx ()

    circle(
        ctx.halfWidth, ctx.halfHeight, 12,
        stroke(Colors.black, 1.0))

    let! angle = useState { 0.0 }
    arc(
        -ctx.width, -ctx.height,
        3 * ctx.width, 3 * ctx.height,
        -90,
        angle.value,
        fill(Colors.darkRed.opacity(0.8)))

    angle.value <- (angle.value + 8.0) % 360.0
}
|> Eval.startFixedFps 50 canvas



// -------------------------------------------------------------



vide {
    bg(Colors.lightGray)

    let! ctx = getCtx ()

    circle(
        ctx.halfWidth, ctx.halfHeight, 12,
        stroke(Colors.black, 1.0))

    let! angle = Anim.linear(1, 0, 360, Repeat.Loop)
    arc(
        -ctx.width, -ctx.height,
        3 * ctx.width, 3 * ctx.height,
        -90,
        angle.value,
        fill(Colors.darkRed.opacity(0.8)))
}
|> Eval.start canvas




// -------------------------------------------------------------



vide {
    bg(Colors.lightGray)

    let! ctx = getCtx ()

    circle(
        ctx.halfWidth, ctx.halfHeight, 12,
        stroke(Colors.black, 1.0))

    let! angle = Anim.linear(1, 0, 360, Repeat.Loop)
    arc(
        -ctx.width, -ctx.height,
        3 * ctx.width, 3 * ctx.height,
        -90,
        angle.value,
        fill(Colors.darkRed.opacity(0.8)))

    text(
        "3",
        5, 2, Colors.black, Fonts.mono16x16, 18.0)
}
|> Eval.start canvas



// -------------------------------------------------------------



vide {
    bg(Colors.lightGray)

    let! ctx = getCtx ()

    circle(
        ctx.halfWidth, ctx.halfHeight, 12,
        stroke(Colors.black, 1.0))

    let! angle = Anim.linear(1, 0, 360, Repeat.Loop)
    arc(
        -ctx.width, -ctx.height,
        3 * ctx.width, 3 * ctx.height,
        -90,
        angle.value,
        fill(Colors.darkRed.opacity(0.8)))

    let overallCountdownSecs = 3
    let! remainingSecs = useState { overallCountdownSecs  }

    text(
        $"{remainingSecs.value}",
        5, 2, Colors.black, Fonts.mono16x16, 18.0)

    let! endReachedOrCrossed = Trigger.falseToTrue angle.isAtEnd
    if endReachedOrCrossed then
        remainingSecs.value <- remainingSecs.value - 1
}
|> Eval.start canvas



// -------------------------------------------------------------


let countdown (overallCountdownSecs: int) =
    vide {
        bg(Colors.lightGray)

        let! ctx = getCtx ()

        circle(
            ctx.halfWidth, ctx.halfHeight, 12,
            stroke(Colors.black, 1.0))

        let! angle = Anim.linear(1, 0, 360, Repeat.Loop)
        arc(
            -ctx.width, -ctx.height,
            3 * ctx.width, 3 * ctx.height,
            -90,
            angle.value,
            fill(Colors.darkRed.opacity(0.8)))

        let! remainingSecs = useState { overallCountdownSecs  }

        text(
            $"{remainingSecs.value}",
            5, 2, Colors.black, Fonts.mono16x16, 18.0)

        let! endReachedOrCrossed = Trigger.falseToTrue angle.isAtEnd
        if endReachedOrCrossed then
            remainingSecs.value <- remainingSecs.value - 1

        return remainingSecs.value <= 0
    }

let plantRising =
    vide {
        let! y = Anim.easeInOutSine(4, 24, 0, Repeat.StopAtEnd)
        image(__SOURCE_DIRECTORY__ </> "plant.png", 0, y.value)
    }

let finalAnimation =
    vide {
        let! hasCountdownFinished = countdown 3

        if hasCountdownFinished then
            bg(Colors.black)
            plantRising
        else 
            keepState
    }

finalAnimation |> Eval.start canvas


// -------------------------------------------------------------


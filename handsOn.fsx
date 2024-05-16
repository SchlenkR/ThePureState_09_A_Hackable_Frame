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
// Evaluate the above code in the F# Interactive (FSI)
// by selecting it and press alt+enter (or cmd+enter on macOS)
// -------------------------------------------------------------




// evaluate that single line (just place the cursor on the line and
// press alt+enter (or cmd+enter on macOS)) to turn off the canvas.
Eval.off canvas



// -------------------------------------------------------------



// A static background
vide {
    bg(Colors.lightGray.opacity(0.5))
}
|> Eval.plot canvas



// -------------------------------------------------------------



// Background, circle and (static) text.
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

    let duration = 3
    let! remainingSecs = useState { duration  }

    text(
        $"{remainingSecs.value}",
        5, 2, Colors.black, Fonts.mono16x16, 18.0)

    let! endReachedOrCrossed = Trigger.falseToTrue angle.isAtEnd
    if endReachedOrCrossed then
        remainingSecs.value <- remainingSecs.value - 1
}
|> Eval.start canvas



// -------------------------------------------------------------


let countdown (duration: int) =
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

        let! remainingSecs = useState { duration  }

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

let finalAnimation duration =
    vide {
        let! hasCountdownFinished = countdown duration

        if hasCountdownFinished then
            bg(Colors.black)
            plantRising
        else 
            keepState
    }

finalAnimation 3 |> Eval.start canvas


// -------------------------------------------------------------


import { _decorator, Component, Node, Graphics, UITransform, Color, Vec3, Vec2, EventTouch, tween, Tween, Label } from 'cc';
const { ccclass, property } = _decorator;

export enum HighlightType {
    CIRCLE,
    RECT
}

@ccclass('GuideMask')
export class GuideMask extends Component {

    @property(Node)
    holeNode: Node = null!;   // Graphics 节点

    @property(Graphics)
    holeGraphics: Graphics = null!;

    @property(Node)
    darkLayer: Node = null!;  // 半透明黑色背景

    @property(Label)
    Guidelabel: Label = null!;

    private runCallback: () => void = null;

    private center = new Vec2();
    private radius = 80;

    private rectW = 100;
    private rectH = 60;

    onLoad() {
    }

    private worldToUI(worldPos: Vec3): Vec2 {
        const uiPos = this.node.getComponent(UITransform)!.convertToNodeSpaceAR(worldPos);
        return new Vec2(uiPos.x, uiPos.y);
    }

    /** 设置圆形高亮 */
    private setCircleHighlight(worldPos: Vec3, cornerRadius) {
        const uiPos = this.worldToUI(worldPos);
        this.center.set(uiPos.x, uiPos.y);
        this.radius = cornerRadius;

        const g = this.holeGraphics;
        if (!g) {
            return;
        }
        g.circle(this.center.x, this.center.y, this.radius);
    }

    /** 设置矩形高亮 */
    private setRectHighlight(worldPos: Vec3, width: number, height: number) {        
        const uiPos = this.worldToUI(worldPos);
        this.rectW = width;
        this.rectH = height;
        this.center.set(uiPos.x, uiPos.y);
        
        const g = this.holeGraphics;
        if (!g) {
            return;
        }
        const x = this.center.x - this.rectW / 2;
        const y = this.center.y - this.rectH / 2;
        g.rect(x, y, this.rectW, this.rectH);
    }


    /** 自动高亮 UI 节点 */
    public highlightNode(targets: [Node, HighlightType][], cb: () => void = null) {
        this.holeNode.active = true;
        this.darkLayer.active = true;

        for (let [target, highlightType] of targets) {
            const ui = target.getComponent(UITransform);
            if (!ui) {
                return;
            }
            
            const size = ui.contentSize;
            const worldPos = target.worldPosition;
            if (highlightType === HighlightType.CIRCLE) {
                const radius = size.height / 2 - 1;
                this.setCircleHighlight(worldPos, radius);
            } else {
                this.setRectHighlight(worldPos, size.width, size.height);
            }
        }
        this.holeGraphics.fill();

        this.runCallback = cb;
    }

    /** 重绘洞 */
    public Redraw() {
        tween(this.node).delay(0.2).repeat(Infinity,
            tween().delay(0.1).call(() => {
                this.runCallback?.call(this);
            }
        )).start();
    }

    public StopHighlight() {
        this.holeNode.active = false;
        this.darkLayer.active = false;
        this.holeGraphics.clear();
        Tween.stopAllByTarget(this.holeGraphics.node);
        this.runCallback = null
    }

    public SetGuideLabel(str: string) {
        this.Guidelabel.string = str;
    }
}
